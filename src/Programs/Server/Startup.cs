namespace Hexalith.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Events;
    using Hexalith.Application.Queries;
    using Hexalith.ApplicationLayer;
    using Hexalith.ApplicationLayer.Helpers;
    using Hexalith.Client;
    using Hexalith.DataIntegrations;
    using Hexalith.Emails;
    using Hexalith.EventStores;
    using Hexalith.GoogleIdentity;
    using Hexalith.Infrastructure.Client;
    using Hexalith.Infrastructure.EfCore.Repositories;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.InMemory.Messages;
    using Hexalith.Infrastructure.Ioc.Commands;
    using Hexalith.Infrastructure.Ioc.Events;
    using Hexalith.Infrastructure.Ioc.Queries;
    using Hexalith.Infrastructure.VisualComponents.MudBlazor.Helpers;
    using Hexalith.Infrastructure.WebServer.Controllers;
    using Hexalith.Infrastructure.WebServer.Exceptions;
    using Hexalith.Infrastructure.WebServer.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;
    using Hexalith.Infrastructure.WebServer.Settings;
    using Hexalith.MexicanDigitalInvoice;
    using Hexalith.MudBlazorTheme;
    using Hexalith.OpenIdDict;
    using Hexalith.QuartzScheduler;
    using Hexalith.Roles;
    using Hexalith.SalesHistory;
    using Hexalith.UblDocuments;
    using Hexalith.Units;
    using Hexalith.Users;
    using Hexalith.WorkItems;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Identity.Web.UI;
    using Microsoft.OpenApi.Models;

    public sealed class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            foreach (var module in GetModuleFactory())
            {
                module
                    .Value(_configuration, _environment)
                    .Configure(app, _environment);
            }
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
                app.UseDirectoryBrowser(new DirectoryBrowserOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        _environment.WebRootPath),
                    RequestPath = "/webroot"
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseBlazorFrameworkFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hexalith V1"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.Map("api/{**slug}", HandleApiFallback);
                endpoints.MapRazorPages();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<StateStoreDbContext>(
                o => o.UseSqlServer(_configuration.GetConnectionString("StateStore")),
                ServiceLifetime.Transient);
            services.AddDirectoryBrowser();
            services.ConfigureSettings<WebServerSettings>(_configuration);
            services.AddServerSideBlazor().AddCircuitOptions(options =>
            {
                if (_environment.IsDevelopment())
                {
                    options.DetailedErrors = true;
                }
            });
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                services.AddScoped(s =>
                {
                    // creating the URI helper needs to wait until the JS Runtime is initialized, so
                    // defer it.
                    var navigationManager = s.GetRequiredService<NavigationManager>();
                    IHttpContextAccessor httpContextAccessor = s.GetRequiredService<IHttpContextAccessor>();
                    if (httpContextAccessor.HttpContext == null)
                    {
                        throw new ServerStartupException("IHttpContextAccessor service has a null valued HttpContext.");
                    }

                    var authToken = httpContextAccessor.HttpContext.Request.Cookies[".AspNetCore.Identity.Application"];
                    HttpClientHandler httpHandler = new() { UseCookies = false };
                    httpHandler.CheckCertificateRevocationList = true;
                    var client = new HttpClient(httpHandler);
                    if (authToken != null)
                    {
                        client.DefaultRequestHeaders.Add("Cookie", ".AspNetCore.Identity.Application=" + authToken);
                    }
                    client.BaseAddress = new Uri(navigationManager.Uri);
                    return client;
                });
            }
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            services.AddControllersWithViews();
            var mvc = services
                .AddRazorPages()
                .AddApplicationPart(typeof(QueryCommandController).Assembly)
                .AddDapr()
                .AddMicrosoftIdentityUI();
            //services.AddServerSideBlazor();
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hexalith", Version = "v1" }));
            var messageFactoryBuilder = new InMemoryMessageFactoryBuilder();
            foreach (var modulePair in GetModuleFactory())
            {
                IServerModule module = modulePair
                    .Value(_configuration, _environment);
                module.ConfigureServices(services);
                services.AddSingleton(module);
                mvc.AddApplicationPart(modulePair.Key.Assembly);
                module.ConfigureMessages(messageFactoryBuilder);
            }
            services.AddSingleton(messageFactoryBuilder.Build());
            services.AddTransient<ICommandBus, IocCommandBus>();
            services.AddTransient<IEventBus, IocEventBus>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddTransient<IQueryBus, IocQueryBus>();
            // services.AddSingleton<IIconRenderer, LineAwesomeIconRenderer>();
            // services.AddSingleton<IComponentRenderer, MudBlazorRenderer>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddWebServer();
            services.AddMudBlazorTheme();
            services.AddApplicationLayer();
        }

        internal static Dictionary<Type, Func<IConfiguration, IWebHostEnvironment, IServerModule>> GetModuleFactory()
        {
            return new Dictionary<Type, Func<IConfiguration, IWebHostEnvironment, IServerModule>>
            {
                { typeof(QuartzSchedulerServerModule), (config, env) => new QuartzSchedulerServerModule(config, env) },
                { typeof(DataIntegrationsServerModule), (config, env) => new DataIntegrationsServerModule(config, env) },
                { typeof(EmailsServerModule), (config, env) => new EmailsServerModule(config, env) },
                { typeof(EventStoresServerModule), (config, env) => new EventStoresServerModule(config, env) },
                { typeof(GoogleIdentityServerModule), (config, env) => new GoogleIdentityServerModule(config, env) },
                //modules.Add(typeof(MicrosoftIdentityServerModule), (config, env) => new MicrosoftIdentityServerModule(config, env));
                { typeof(OpenIdDictServerModule), (config, env) => new OpenIdDictServerModule(config, env) },
                { typeof(RolesServerModule), (config, env) => new RolesServerModule(config, env) },
                { typeof(UblDocumentsServerModule), (config, env) => new UblDocumentsServerModule(config, env) },
                { typeof(MexicanDigitalInvoiceServerModule), (config, env) => new MexicanDigitalInvoiceServerModule(config, env) },
                { typeof(UsersServerModule), (config, env) => new UsersServerModule(config, env) },
                { typeof(UnitsServerModule), (config, env) => new UnitsServerModule(config, env) },
                { typeof(WorkItemsServerModule), (config, env) => new WorkItemsServerModule(config, env) },
                { typeof(SalesHistoryServerModule), (config, env) => new SalesHistoryServerModule(config, env) },
                { typeof(MudBlazorThemeServerModule), (config, env) => new MudBlazorThemeServerModule(config, env) },
                { typeof(ApplicationLayerServerModule), (config, env) => new ApplicationLayerServerModule(config, env) }
            };
        }

        private static Task HandleApiFallback(HttpContext context)
        {
            // Calling an API that does not exist.
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            return Task.CompletedTask;
        }
    }
}
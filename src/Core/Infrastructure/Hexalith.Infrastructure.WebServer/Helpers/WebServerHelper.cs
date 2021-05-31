namespace Hexalith.Infrastructure.WebServer.Helpers
{
    using System.Security.Principal;

    using Hexalith.Application.Commands;
    using Hexalith.Application.Identities;
    using Hexalith.Application.Queries;
    using Hexalith.Application.Services;
    using Hexalith.Infrastructure.VisualComponents.Renderers;
    using Hexalith.Infrastructure.WebServer.Exceptions;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebServerHelper
    {
        public static void AddWebServer(this IServiceCollection services)
        {
            services.AddTransient<IComponentRendererProvider, ComponentRendererProvider>();
            services.AddScoped<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? throw new HttpContextNotFoundException());
            services.AddScoped<IUserIdentity, UserIdentity>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<ICommandService, CommandService>();
        }
    }
}
// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Emails.SendGrid.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;

using HexalithApplication.Components;
using HexalithApplication.Components.Account;
using HexalithApplication.Data;
using HexalithApplication.Helpers;
using HexalithApplication.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

using Serilog;

const string appName = "Hexalith Application";

#if DEBUG
bool debugInVisualStudio = true;
#else
bool debugInVisualStudio = false;
#endif

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    appName,
    "v1",
    debugInVisualStudio,
    (actors) => { },
    args);

// Add email services for sending authentication emails.
builder.Services.AddSendGridEmail(builder.Configuration);
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.ConfigureSettings<OrganizationSettings>(builder.Configuration);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddMicrosoftAccount(microsoftOptions =>
    {
        string? clientId = builder.Configuration["Authentication:Microsoft:ClientId"];
        string? clientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new InvalidOperationException("Authentication:Microsoft:ClientId must be set in the application settings.");
        }

        if (string.IsNullOrWhiteSpace(clientSecret))
        {
            throw new InvalidOperationException("Authentication:Microsoft:ClientSecret must be set in the application settings.");
        }

        microsoftOptions.ClientId = clientId;
        microsoftOptions.ClientSecret = clientSecret;
    })
    .AddIdentityCookies();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("api", p =>
    {
        p.RequireAuthenticatedUser();
        p.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders()
    .AddApiEndpoints();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    _ = app.UseMigrationsEndPoint();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseHexalith();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HexalithApplication.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.MapGroup("api/auth").MapIdentityApi<ApplicationUser>();

Log.Logger.Information("Starting {AppName}.", appName);

try
{
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Error starting {AppName}.", appName);
    throw;
}
finally
{
    Log.Logger.Information("{AppName}, is stopped.", appName);
}
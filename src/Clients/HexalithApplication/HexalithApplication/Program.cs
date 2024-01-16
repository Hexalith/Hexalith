// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Application.Commands;
using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.Infrastructure.Emails.SendGrid.Helpers;

using HexalithApplication.Components;
using HexalithApplication.Components.Account;
using HexalithApplication.Data;
using HexalithApplication.Services;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSingleton<ICommandBus, DaprCommandBus>();
builder.Services.AddDaprClient();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
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

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityEmailSender>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    _ = app.UseMigrationsEndPoint();
}
else
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HexalithApplication.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Identity.Web;

using Radzen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container for Radzen UI.
builder.Services.AddRadzenComponents();

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddMicrosoftIdentityConsentHandler();

// Configuring appsettings section AzureAdB2C, into IOptions
builder.Services.AddOptions();
builder.Services.Configure<OpenIdConnectOptions>(builder.Configuration.GetSection("AzureAd"));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;

    // Handling SameSite cookie according to https://learn.microsoft.com/aspnet/core/security/samesite?view=aspnetcore-3.1
    _ = options.HandleSameSiteCookieCompatibility();
});

builder
    .Services
    .AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
    .EnableTokenAcquisitionToCallDownstreamApi(["user.read"])
    .AddMicrosoftGraph()
    .AddInMemoryTokenCaches();

// builder.Services.AddRazorPages().AddMvcOptions(options =>
// {
//    AuthorizationPolicy policy = new AuthorizationPolicyBuilder()
//                  .RequireAuthenticatedUser()
//                  .Build();
//    options.Filters.Add(new AuthorizeFilter(policy));
// }).AddMicrosoftIdentityUI();
builder.Services.AddAuthorization(options => options.FallbackPolicy = options.DefaultPolicy);

// builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

// builder.Services
//    .AddControllersWithViews()
//    .AddMicrosoftIdentityUI();
WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

// Add the Microsoft Identity Web cookie policy
app.UseCookiePolicy();

app.UseRouting();

// Add the ASP.NET Core authentication service
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<HexalithApp.Components.App>()
    .AddInteractiveServerRenderMode();

// .AddAdditionalAssemblies(typeof(Error).Assembly);
app.Run();
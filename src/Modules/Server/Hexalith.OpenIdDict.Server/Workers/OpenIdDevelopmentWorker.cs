using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Infrastructure;
using Hexalith.OpenIdDict.Data;
using Hexalith.OpenIdDict.Settings;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using OpenIddict.Abstractions;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Hexalith.OpenIdDict.Workers
{
    public class OpenIdDevelopmentWorker : IHostedService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IServiceProvider _serviceProvider;

        public OpenIdDevelopmentWorker(IServiceProvider serviceProvider, IWebHostEnvironment environment)
        {
            _serviceProvider = serviceProvider;
            _environment = environment;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<SecurityDbContext>();
            var settings = scope.ServiceProvider.GetRequiredService<IOptions<OpenIdSettings>>();
            await context.Database.EnsureCreatedAsync(cancellationToken);
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var authorizedUrls = new List<string>(settings.Value.AuthorizedUrls??Array.Empty<string>());
            if (authorizedUrls.Count == 0 && _environment.IsDevelopment())
            {
                authorizedUrls.AddRange(new[] { "https://localhost:5001", "http://localhost:5000", "https://localhost:6001", "http://localhost:6000" });
            }
            if (await manager.FindByClientIdAsync(HexalithConstants.ServerApiName, cancellationToken) is null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = HexalithConstants.ServerApiName,
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = nameof(Hexalith) + " client application",
                    Type = ClientTypes.Public,
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                };

                foreach (var uri in authorizedUrls
                    .Select(p => new Uri(new Uri(p), "authentication/logout-callback")))
                {
                    descriptor.PostLogoutRedirectUris.Add(uri);
                }

                foreach (var uri in authorizedUrls
                     .Select(p => new Uri(new Uri(p), "authentication/login-callback")))
                {
                    descriptor.RedirectUris.Add(uri);
                }
                await manager.CreateAsync(descriptor, cancellationToken);
            }
            const string powerBiClient = "PowerBIClient";
            if (await manager.FindByClientIdAsync(powerBiClient, cancellationToken) is null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = powerBiClient,
                    ClientSecret = "C01CAEEF-DC17-4D1E-B054-3422BD330047",
                    DisplayName = "Power BI client application",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials,
                    }
                };
                await manager.CreateAsync(descriptor, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
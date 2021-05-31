namespace Hexalith.Infrastructure.MicrosoftGraph.Tests.Fixture
{
    using System;

    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.MicrosoftGraph.Settings;

    using Microsoft.Extensions.Configuration;

    public class GraphFixture
    {
        private GraphAuthenticationService _authenticationService;
        private GraphService _graphService;
        private MicrosoftGraphAuthenticationSettings _settings;
        public MicrosoftGraphAuthenticationSettings Settings => _settings ??= GetSettings();

        internal GraphAuthenticationService AuthenticationService => _authenticationService ??= InitializeService();
        internal GraphService GraphService => _graphService ??= InitializeGraphService();

        public static string GetTestEmail()
                    => new ConfigurationBuilder()
                .AddUserSecrets<GraphAutenticationServiceTest>()
                .Build()
                .GetValue<string>("TestEmail");

        public GraphService InitializeGraphService()
                    => new(AuthenticationService);

        private static MicrosoftGraphAuthenticationSettings GetSettings()
                    => new ConfigurationBuilder()
                .AddUserSecrets<GraphAutenticationServiceTest>()
                .Build()
                .GetSettings<MicrosoftGraphAuthenticationSettings>();

        private GraphAuthenticationService InitializeService() => string.IsNullOrWhiteSpace(Settings.TenantId) ||
                string.IsNullOrWhiteSpace(Settings.ClientId) ||
                string.IsNullOrWhiteSpace(Settings.ClientSecret)
                ? throw new Exception("Configuration error")
                : new GraphAuthenticationService(Settings.TenantId, Settings.ClientId, Settings.ClientSecret, Settings.Authority);
    }
}
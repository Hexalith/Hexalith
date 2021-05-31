namespace Hexalith.Infrastructure.MicrosoftGraph
{
    using Microsoft.Graph.Auth;
    using Microsoft.Identity.Client;

    public class GraphAuthenticationService
    {
        private ClientCredentialProvider? _authenticationProvider;
        private IConfidentialClientApplication? _confidentialClientApplication;

        public GraphAuthenticationService(string tenantId, string clientId, string clientSecret, string? authority)
        {
            TenantId = tenantId;
            ClientId = clientId;
            ClientSecret = clientSecret;
            Authority = authority;
        }

        public ClientCredentialProvider AuthenticationProvider
            => _authenticationProvider ??= InitializeAutheticationProvider();

        public string? Authority { get; }
        public string ClientId { get; }

        public string ClientSecret { get; }

        public string TenantId { get; }

        protected IConfidentialClientApplication ConfidentialClientApplication
            => _confidentialClientApplication ??= InitializeClientApplication();

        private ClientCredentialProvider InitializeAutheticationProvider()
            => new(ConfidentialClientApplication);

        private IConfidentialClientApplication InitializeClientApplication()
        {
            var builder = ConfidentialClientApplicationBuilder
                .Create(ClientId)
                .WithTenantId(TenantId)
                .WithClientSecret(ClientSecret);
            if (!string.IsNullOrWhiteSpace(Authority))
            {
                builder.WithAuthority(Authority);
            }
            return builder.Build();
        }
    }
}
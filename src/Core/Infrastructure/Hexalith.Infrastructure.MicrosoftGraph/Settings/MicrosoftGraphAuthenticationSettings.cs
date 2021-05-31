namespace Hexalith.Infrastructure.MicrosoftGraph.Settings
{
    public class MicrosoftGraphAuthenticationSettings
    {
        public string? Authority { get; init; }
        public string? ClientId { get; init; }

        public string? ClientSecret { get; init; }

        public string? TenantId { get; init; }
    }
}
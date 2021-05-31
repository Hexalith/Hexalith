using System;

using Microsoft.Identity.Client;

namespace Hexalith.MicrosoftIdentity
{
    public class AzureAdSettings
    {
        public string? CallbackPath { get; init; }
        public string? ClientId { get; init; }
        public string? Instance { get; init; }
        public string? SignedOutCallbackPath { get; init; }
        public string? TenantId { get; init; }
    };

    public class MicrosoftGraphSettings
    {
        public string? BaseUrl { get; init; }
        public string? MicrosoftGraph { get; init; }
        public string? Scopes { get; init; }
    };

    public class MicrosoftIdentitySettings
    {
        public AzureAdSettings AzureAd { get; init; } = new AzureAdSettings();
        public MicrosoftGraphSettings MicrosoftGraph { get; init; } = new MicrosoftGraphSettings();
    }
}
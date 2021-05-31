namespace Hexalith.OpenIdDict.Settings
{
    using System.Collections.Generic;

    public class OpenIdSettings
    {
        public IEnumerable<string>? AuthorizedUrls { get; init; }
        public IEnumerable<string>? CertificatePaths { get; init; }
        public string? EncryptionCertificateFile { get; init; }
        public string? EncryptionCertificateThumbprint { get; init; }
        public string? EncryptionCertificateFilePassword { get; init; }
        public string? SigningCertificateFile { get; init; }
        public string? SigningCertificateFilePassword { get; init; }
        public string? SigningCertificateThumbprint { get; init; }
        public string? SigningCertificateThumbprintPassword { get; init; }
        public bool AllowGoogleAuthentication { get; init; }
        public bool AllowFacebookAuthentication { get; init; }
        public bool AllowMicrosoftAuthentication { get; init; }
    }
}
namespace Hexalith.Tools
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    internal static class Program
    {
        private enum KeyType
        {
            Signing,
            Encryption
        }

        public static void WriteToFile(string fileName, byte[] content)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            using var writer = File.Create(path);
            writer.Write(content);
            Console.WriteLine("Certificate file created : " + path);
        }

        private static void CreateCertificate(KeyType keyType)
        {
            using var algorithm = RSA.Create(keySizeInBits: 2048);

            var request = new CertificateRequest($"CN=Hexalith {keyType} Authority", algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509KeyUsageExtension(
                keyType == KeyType.Encryption ?
                    X509KeyUsageFlags.KeyEncipherment :
                    X509KeyUsageFlags.DigitalSignature,
                critical: true));

            var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

            // Note: setting the friendly name is not supported on Unix machines (including Linux
            // and macOS). To ensure an exception is not thrown by the property setter, an OS
            // runtime check is used here.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                certificate.FriendlyName = $"Hexalith OpenId {keyType} Certificate";
            }

            // Note: Verify that the application has access to the private key. On the certificate,
            // right click 'All tasks/Manage private' keys and add the application service user.
            var data = certificate.Export(X509ContentType.Pfx, "Hexalith_45AZER");
            WriteToFile($"{nameof(Hexalith)}{keyType}Certificate.pfx", data);
        }

        private static void Main()
        {
            CreateCertificate(KeyType.Encryption);
            CreateCertificate(KeyType.Signing);
        }
    }
}
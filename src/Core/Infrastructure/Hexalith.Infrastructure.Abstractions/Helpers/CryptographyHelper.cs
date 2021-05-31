namespace Hexalith.Infrastructure.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class CryptographyHelper
    {
        public static string ToMD5Base64(this string text)
        {
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
            using MD5 hash = MD5.Create();
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms

            return ToBase64(hash.ComputeHash(Encoding.Default.GetBytes(text)));
        }

        public static string ToSha256Base64(this string text)
        {
            using SHA256 hash = SHA256.Create();

            return ToBase64(hash.ComputeHash(Encoding.Default.GetBytes(text)));
        }

        public static string ToSha512Base64(this string text)
        {
            using SHA512 hash = SHA512.Create();

            return ToBase64(hash.ComputeHash(Encoding.Default.GetBytes(text)));
        }

        private static string ToBase64(byte[] bytes)
            => Convert.ToBase64String(bytes).TrimEnd('=');
    }
}
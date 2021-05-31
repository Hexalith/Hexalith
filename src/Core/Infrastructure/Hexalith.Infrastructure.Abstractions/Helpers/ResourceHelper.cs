using System;
using System.IO;
using System.Reflection;

namespace Hexalith.Infrastructure.Helpers
{
    public static class ResourceHelper
    {
        public static byte[] GetResource(this Assembly assembly, string name)
        {
            using var stream = assembly.GetManifestResourceStream(name);
            if (stream == null)
            {
                throw new InvalidOperationException($"Resource '{name}' not found in {assembly.FullName}.");
            }
            var bytes = new byte[stream.Length];
            using (var memoryStream = new MemoryStream(bytes))
            {
                stream.CopyTo(memoryStream);
            }

            return bytes;
        }
    }
}
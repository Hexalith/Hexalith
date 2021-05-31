namespace Hexalith.Domain.ValueTypes
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{Value}")]
    public class AutoIdentifier : StringValue
    {
        public AutoIdentifier(AutoIdentifier autoIdentifier)
        {
            Value = string.IsNullOrWhiteSpace(autoIdentifier?.Value) ? GenerateIdentifier() : autoIdentifier.Value;
        }

        public AutoIdentifier(string id)
        {
            Value = string.IsNullOrWhiteSpace(id) ? GenerateIdentifier() : id.Trim();
        }

        public AutoIdentifier()
        {
            Value = GenerateIdentifier();
        }

        public static string GenerateIdentifier()
            => Convert
                .ToBase64String(Guid.NewGuid().ToByteArray())
                .Substring(0, 22)
                .Replace("/", "_", StringComparison.InvariantCulture)
                .Replace("+", "-", StringComparison.InvariantCulture);
    }
}
namespace Hexalith.Infrastructure.VisualComponents.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public enum Appearance
    {
        Neutral,
        Accent,
        Hypertext,
        Lightweight,
        Outline,
        Stealth,
        Filled
    }

    internal static class AppearanceExtensions
    {
        private static Dictionary<Appearance, string> _appearanceValues =
            Enum.GetValues<Appearance>().ToDictionary(id => id, id => Enum.GetName(id)?.ToLowerInvariant() ?? string.Empty);

        public static string ToAttributeValue(this Appearance? value) => value == null ? string.Empty : _appearanceValues[value.Value];
    }
}
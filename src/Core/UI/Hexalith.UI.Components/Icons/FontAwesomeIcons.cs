namespace Hexalith.UI.Components.Icons;

using Hexalith.UI.Components.Helpers;

using Microsoft.FluentUI.AspNetCore.Components;

public static class FontAwesomeIcons
{
    public static Icon GetIcon(IconInformation info)
    {
        ArgumentNullException.ThrowIfNull(info);
        string content = $"""
            <svg class="fluent-nav-icon" style="width: {info.Size}px; fill: var(--accent-fill-rest);" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
              <use href="./_content/{info.IconLibraryName}/fontawesome/sprites/{GetStylePath(info.Style)}.svg#{info.Name.ToLowerInvariant()}"></use>
            </svg>
            """;
        Icon icon = new Icon(
            info.Name,
            info.Style switch { IconStyle.Regular => IconVariant.Regular, IconStyle.Filled => IconVariant.Filled, IconStyle.Light => IconVariant.Light, IconStyle.Thin => IconVariant.Light, _ => IconVariant.Regular },
            IconHelper.GetIconSize(info.Size),
            content);
        return icon;
    }

    private static string GetStylePath(IconStyle style)
    {
        return style switch
        {
            IconStyle.Regular => "regular",
            IconStyle.Filled => "solid",
            IconStyle.Light => "light",
            IconStyle.Thin => "thin",
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, "Unsupported style for Fontawesome icons."),
        };
    }

    private static string GetSizeClassName(int size)
    {
        return size switch
        {
            <= 10 => "fa-2xs",
            <= 12 => "fa-xs",
            <= 14 => "fa-sm",
            <= 20 => "fa-lg",
            <= 24 => "fa-xl",
            <= 32 => "fa-2xl",
            _ => "fa-2xl",
        };
    }
}
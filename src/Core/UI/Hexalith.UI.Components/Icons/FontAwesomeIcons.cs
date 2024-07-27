namespace Hexalith.UI.Components.Icons;

using Hexalith.UI.Components.Helpers;

using Microsoft.FluentUI.AspNetCore.Components;

public static class FontAwesomeIcons
{
    public static Icon GetIcon(IconInformation info)
    {
        return new Icon(
            info.Name,
            info.Style switch { IconStyle.Regular => IconVariant.Regular, IconStyle.Filled => IconVariant.Filled, IconStyle.Light => IconVariant.Light, IconStyle.Thin => IconVariant.Light, _ => IconVariant.Regular },
            IconHelper.GetIconSize(info.Size),
            $"<span class=\"{GetAspectClassName(info.Aspect)}{GetStyleClassName(info.Style)}fa-{info.Name.ToLowerInvariant()} {GetSizeClassName(info.Size)}\"></span>"
            );
    }

    private static string GetStyleClassName(IconStyle style)
    {
        return style switch
        {
            IconStyle.Regular => "fa-regular",
            IconStyle.Filled => "fa-solid",
            IconStyle.Light => "fa-light",
            IconStyle.Thin => "fa-thin",
            _ => "fa-regular",
        };
    }

    private static string GetAspectClassName(IconAspect aspect)
    {
        return aspect switch
        {
            IconAspect.Duotone => "fa-duotone ",
            IconAspect.Sharp => "fa-sharp ",
            IconAspect.SharpDuotone => "fa-sharp-duotone ",
            _ => string.Empty,
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
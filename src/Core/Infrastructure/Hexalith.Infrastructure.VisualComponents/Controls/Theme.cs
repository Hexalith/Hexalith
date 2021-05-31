namespace Hexalith.Infrastructure.VisualComponents.Controls
{
    using System;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Rendering;

    public class Theme : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; } = default!;

        [CascadingParameter(Name = nameof(ThemeName))]
        public string ThemeName { get; set; } = "Fast";

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            int sequence = 0;
            if (ChildContent != null && !string.IsNullOrWhiteSpace(ThemeName))
            {
                if (ThemeName == "Fast")
                {
                    builder.OpenElement(sequence++, "fast-design-system-provider");
                }
                else if (ThemeName == "Fluent")
                {
                    builder.OpenElement(sequence++, "fluent-design-system-provider");
                }
                builder.AddAttribute(sequence++, "use-defaults");
                builder.AddContent(sequence++, ChildContent);
                builder.CloseElement();
                if (ThemeName == "Fast")
                {
                    builder.OpenElement(sequence++, "script");
                    builder.AddAttribute(sequence++, "type", "module");
                    builder.AddAttribute(sequence++, "src", "https://unpkg.com/@microsoft/fast-components");
                    builder.CloseElement();
                }
                else if (ThemeName == "Fluent")
                {
                    builder.OpenElement(sequence++, "script");
                    builder.AddAttribute(sequence++, "type", "module");
                    builder.AddAttribute(sequence++, "src", "https://unpkg.com/@fluentui/web-components");
                    builder.CloseElement();
                }
            }
        }
    }
}
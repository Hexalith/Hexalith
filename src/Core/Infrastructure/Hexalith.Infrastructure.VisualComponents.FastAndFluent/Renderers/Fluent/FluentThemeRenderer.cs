namespace Bistrotic.Infrastructure.VisualComponents.Renderers.Fluent
{
    using Bistrotic.Infrastructure.VisualComponents;
    using Bistrotic.Infrastructure.VisualComponents.Controls;

    using Microsoft.AspNetCore.Components.Rendering;

    public sealed record FluentThemeRenderer : FluentComponentRendererBase<Theme>
    {
        public FluentThemeRenderer() : base("fluent-design-system-provider")
        {

        }
        protected override int RenderElement(int sequence, BlazorComponent blazorComponent, RenderTreeBuilder builder)
        {
            sequence = base.RenderElement(sequence, blazorComponent, builder);
            builder.OpenElement(sequence++, "script");
            builder.AddAttribute(sequence++, "type", "module");
            builder.AddAttribute(sequence++, "src", "https://unpkg.com/@fluentui/web-components");
            builder.CloseElement();
            return sequence;
        }
        protected override int RenderAttributes(int sequence, BlazorComponent blazorComponent, RenderTreeBuilder builder)
        {
            if (blazorComponent.AdditionalAttributes == null || blazorComponent.AdditionalAttributes.Count == 0)
                builder.AddAttribute(sequence++, "use-defaults");
            else
                builder.AddMultipleAttributes(sequence++, blazorComponent.AdditionalAttributes);
            return base.RenderAttributes(sequence, blazorComponent, builder);
        }

    }
}
namespace Bistrotic.Infrastructure.VisualComponents.Renderers
{
    using System;

    using Microsoft.AspNetCore.Components.Rendering;

    public abstract record ComponentRendererBase<TComponent> : IComponentRenderer<TComponent>
        where TComponent : BlazorComponent
    {
        public ComponentRendererBase(string themeName, string? componentName = null)
        {
            ThemeName = themeName;
            if (componentName == null)
            {
                ComponentName = ThemeName.ToLowerInvariant().Trim() + "-" + typeof(TComponent).Name.DashCase();
            }
            else
            {
                ComponentName = componentName;
            }
        }

        public string ThemeName { get; }
        protected string ComponentName { get; }

        public virtual void BuildRenderTree(BlazorComponent blazorComponent, RenderTreeBuilder builder)
        {
            RenderElement(0, blazorComponent, builder);
        }

        protected virtual int RenderElement(int sequence, BlazorComponent blazorComponent, RenderTreeBuilder builder)
        {
            builder.OpenElement(sequence++, ComponentName);
            sequence = RenderAttributes(sequence, blazorComponent, builder);
            sequence = blazorComponent.RenderContent(sequence, builder);
            builder.CloseElement();
            return sequence;
        }
        protected virtual int RenderAttributes(int sequence, BlazorComponent blazorComponent, RenderTreeBuilder builder)
        {
            return sequence;
        }

        public Type ControlType => typeof(TComponent);
    }
}
namespace Bistrotic.Infrastructure.VisualComponents.Renderers.Fluent
{
    using Bistrotic.Infrastructure.VisualComponents.Renderers;

    public abstract record FluentComponentRendererBase<TComponent> : ComponentRendererBase<TComponent>
        where TComponent : BlazorComponent
    {
        public FluentComponentRendererBase(string? componentName = null) : base(nameof(Fluent), componentName)
        {
        }

        protected FluentComponentRendererBase(string themeName, string? componentName = null) : base(themeName, componentName)
        {
        }
    }
}
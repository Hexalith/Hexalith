#pragma warning disable CS3024 // Constraint type is not CLS-compliant

namespace Hexalith.Infrastructure.VisualComponents.Renderers
{
    using System;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Rendering;

    public interface IComponentRenderer
    {
        public Type ControlType { get; }

        public string ThemeName { get; }

        void BuildRenderTree(IComponent blazorComponent, RenderTreeBuilder builder);
    }

    public interface IComponentRenderer<TComponent> : IComponentRenderer
        where TComponent : IComponent
    {
    }
}
#pragma warning disable CS3024 // Constraint type is not CLS-compliant

namespace Hexalith.Infrastructure.VisualComponents.Renderers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Components;

    public interface IComponentRendererProvider
    {
        IEnumerable<string> ThemeNames { get; }

        IComponentRenderer<TComponent>? GetRenderer<TComponent>(string themeName) where TComponent : IComponent;

        IComponentRenderer? GetRenderer(string themeName, Type componentType);
    }
}
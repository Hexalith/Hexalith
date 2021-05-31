
namespace Bistrotic.Infrastructure.VisualComponents.FastAndFluent.Helpers
{
    using Bistrotic.Infrastructure.VisualComponents.Renderers;
    using Bistrotic.Infrastructure.VisualComponents.Renderers.Fast;
    using Bistrotic.Infrastructure.VisualComponents.Renderers.Fluent;

    using Microsoft.Extensions.DependencyInjection;

    public static class FastAndFluentRendererHelper
    {
        public static void AddFastRenderers(this IServiceCollection services)
        {
            services.AddTransient<IComponentRenderer, FastThemeRenderer>();
        }
        public static void AddFluentRenderers(this IServiceCollection services)
        {
            services.AddTransient<IComponentRenderer, FluentThemeRenderer>();
        }
    }
}

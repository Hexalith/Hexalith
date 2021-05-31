using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hexalith.Infrastructure.WebServer.Settings
{
    public class WebServerSettings
    {
        public RenderMode ClientMode { get; init; } = RenderMode.Server;
    }
}
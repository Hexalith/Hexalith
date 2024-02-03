namespace Hexalith.Infrastructure.ClientAppOnServer.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hexalith.Infrastructure.ClientApp.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class WebAssemblyClientHelper
{
    public static IServiceCollection AddHexalithWasmClientApp(IServiceCollection services, IConfiguration configuration)
    {
        return services.AddHexalithClientApp(configuration);
    }
}
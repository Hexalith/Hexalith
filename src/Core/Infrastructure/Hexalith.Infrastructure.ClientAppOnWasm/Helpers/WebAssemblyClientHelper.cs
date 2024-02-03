// <copyright file="WebAssemblyClientHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Helpers;

using Hexalith.Infrastructure.ClientApp.Helpers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class WebAssemblyClientHelper
{
    public static IServiceCollection AddHexalithWasmClientApp(this IServiceCollection services, IConfiguration configuration)
        => services.AddHexalithClientApp(configuration);
}
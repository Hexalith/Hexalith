// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Dapr.Actors.Client;

using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Aggregates.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.WebApis.ExternalSystemsCommands.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;

using Microsoft.Extensions.DependencyInjection.Extensions;

using Serilog;

const string appDescription = "Hexalith External Systems";
#if DEBUG
bool debugInVisualStudio = true;
#else
bool debugInVisualStudio = false;
#endif

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    appDescription,
    "v1",
    debugInVisualStudio,
    (actors) => actors.AddExternalSystemsAggregates(),
    args);

builder.Services.AddExternalSystemsCommands();

WebApplication app = builder.Build();

app.UseHexalith();

Log.Logger.Information("Starting {AppName}.", appDescription);

try
{
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Error starting {AppName}.", appDescription);
    throw;
}
finally
{
    Log.Logger.Information("{AppName}, is stopped.", appDescription);
}
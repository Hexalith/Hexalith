// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.AspireService.Defaults;
using Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Infrastructure.WebApis.PartiesCommands.Helpers;

const string appName = "Hexalith Parties";

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    appName,
    "v1",
    (actors) => actors.AddPartiesAggregates(),
    args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

builder.Services.AddPartiesCommandsSubmission();

WebApplication app = builder.Build();

app.UseHexalith();

ILogger<Program> logger = app.Services.GetRequiredService<ILogger<Program>>();
#pragma warning disable CA1848 // Use the LoggerMessage delegates
try
{
    logger.LogInformation("Starting {AppName}.", appName);
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    logger.LogError(ex, "Error starting {AppName}.", appName);
    throw;
}
finally
{
    logger.LogInformation("{AppName}, is stopped.", appName);
}
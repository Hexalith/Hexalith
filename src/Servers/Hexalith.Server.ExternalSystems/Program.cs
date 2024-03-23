// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Aggregates.Helpers;
using Hexalith.Infrastructure.WebApis.ExternalSystemsCommands.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Server.ExternalSystems;

const string appName = "Hexalith External Systems";

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    appName,
    "v1",
    (actors) => actors.AddExternalSystemsAggregates(),
    args);

builder.Services.AddExternalSystemsCommandsSubmission();

await builder
    .Build()
    .UseHexalith<Program>(ExternalSystemsConstants.ApplicationName)
    .RunAsync().ConfigureAwait(false);
// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Infrastructure.WebApis.PartiesCommands.Helpers;
using Hexalith.Server.Parties;

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    PartiesConstants.ApplicationName,
    "v1",
    (actors) => actors.AddPartiesAggregates(),
    args);

builder.Services.AddPartiesCommandsSubmission();

await builder
    .Build()
    .UseHexalith<Program>(PartiesConstants.ApplicationName)
    .RunAsync()
    .ConfigureAwait(false);
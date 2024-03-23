// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.DaprRuntime.Inventories.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Infrastructure.WebApis.InventoriesCommands.Helpers;
using Hexalith.Server.Inventories;

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    InventoriesConstants.ApplicationName,
    "v1",
    (actors) => actors.AddInventoriesAggregates(),
    args);

builder.Services.AddInventoriesCommandsSubmission();

await builder
    .Build()
    .UseHexalith<Program>(InventoriesConstants.ApplicationName)
    .RunAsync()
    .ConfigureAwait(false);
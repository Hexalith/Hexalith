// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.DaprRuntime.Sales.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Infrastructure.WebApis.SalesCommands.Helpers;
using Hexalith.Server.Sales;

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    SalesConstants.ApplicationName,
    "v1",
    (actors) => actors.AddSalesAggregates(),
    args);

builder.Services.AddSalesCommandsSubmission();

await builder
    .Build()
    .UseHexalith<Program>(SalesConstants.ApplicationName)
    .RunAsync()
    .ConfigureAwait(false);
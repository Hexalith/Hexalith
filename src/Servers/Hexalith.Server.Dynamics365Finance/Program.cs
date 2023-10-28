// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Helpers;
using Hexalith.Infrastructure.WebApis.ExternalSystems.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;

using Serilog;

const string applicationDescription = "Hexalith Dynamics 365 Finance and Operations";

#if DEBUG
bool debugInVisualStudio = true;
#else
bool debugInVisualStudio = false;
#endif
const string applicationName = "Dynamics365Finance";
WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    applicationDescription,
    "v1",
    debugInVisualStudio,
    (actors) => actors.AddExternalSystemsMapper(applicationName, Customer.GetAggregateName()),
    args);

builder.Services.AddDynamics365FinanceCustomers(builder.Configuration);
builder.Services.AddDaprPartiesClient();
builder.Services.AddDaprExternalSystemsMapper(applicationName, Customer.GetAggregateName());
builder.Services.AddExternalSystemsMapperUpdate(applicationName);
builder.Services.AddOrganizations(builder.Configuration);
builder.Services.AddSingleton<IIntegrationEventProcessor, IntegrationEventProcessor>();
builder.Services.AddSingleton<IIntegrationEventDispatcher, DependencyInjectionEventDispatcher>();

WebApplication app = builder.Build();

app.UseHexalith();

Log.Logger.Information("Starting {AppName}.", applicationDescription);

try
{
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Error starting {AppName}.", applicationDescription);
    throw;
}
finally
{
    Log.Logger.Information("{AppName}, is stopped.", applicationDescription);
}
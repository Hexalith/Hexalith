// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Helpers;
using Hexalith.Infrastructure.WebApis.ExternalSystemsEvents.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers;
using Hexalith.Server.Dynamics365Finance;

using Serilog;

const string applicationDescription = "Hexalith Dynamics 365 Finance and Operations";

#if DEBUG
bool debugInVisualStudio = true;
#else
bool debugInVisualStudio = false;
#endif

IEnumerable<string> aggregateNames = [Customer.GetAggregateName()];
WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    applicationDescription,
    "v1",
    debugInVisualStudio,
    (actors) =>
    {
        actors.AddExternalSystemsMapper(Dynamics365FinanceConstants.ApplicationName, aggregateNames);
        actors.AddPartiesProjections(Dynamics365FinanceConstants.ApplicationName);
        actors.AddDynamics365FinanceProjections(Dynamics365FinanceConstants.ApplicationName);
    },
    args);

builder.Services
    .AddCustomerProjections(Dynamics365FinanceConstants.ApplicationName)
    .AddExternalSystemsMapperSubscription(Dynamics365FinanceConstants.ApplicationName, aggregateNames)
    .AddDynamics365FinanceCustomers(builder.Configuration, Dynamics365FinanceConstants.ApplicationName)
    .AddOrganizations(builder.Configuration)
    .AddScoped<IIntegrationEventHandler<CustomerRegistered>, CustomerRegisteredHandler>()
    .AddScoped<IIntegrationEventHandler<CustomerInformationChanged>, CustomerInformationChangedHandler>()
    .AddScoped<IIntegrationEventProcessor, IntegrationEventProcessor>()
    .AddScoped<IIntegrationEventDispatcher, DependencyInjectionEventDispatcher>();

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

/* Dynamics Customers Business Event sample
{
  "Account": "6043",
  "BusinessEventId": "FFYCustomerChangedBusinessEvent",
  "BusinessEventLegalEntity": "0031",
  "CommissionSalesGroupId": "",
  "Contact": {
    "Email": "",
    "Mobile": "",
    "Person": null,
    "Phone": "",
    "PostalAddress": {
      "City": "New York",
      "CountryId": "USA",
      "CountryIso2": "US",
      "CountryName": "United States",
      "StateId": "NY",
      "StateName": "New York",
      "Street": "1000 Third Avenue",
      "StreetNumber": "",
      "ZipCode": "10022"
    }
  },
  "ContextRecordSubject": "",
  "ControlNumber": 5637985363,
  "EventId": "9C091AF1-5BAF-4CA9-B0A4-94D6E59A0D1C",
  "EventTime": "/Date(1697470966000)/",
  "EventTimeIso8601": "2023-10-16T15:42:46.3440655Z",
  "ExternalReferences": [
    { "ExternalId": "Bloomingdales", "SystemId": "Spring" }
  ],
  "InitiatingUserAADObjectId": "{C600136F-6814-41D9-8642-40CA0EC4EDD4}",
  "InterCompanyDirectDelivery": "Yes",
  "LegalEntity": "0031",
  "MajorVersion": 0,
  "MinorVersion": 0,
  "Name": "Bloomingdale's 0001-NY",
  "ParentContextRecordSubjects": [],
  "WarehouseId": "3199-DD"
}

 */
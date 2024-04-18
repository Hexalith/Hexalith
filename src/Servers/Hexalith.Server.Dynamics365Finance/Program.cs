// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Application.Events;
using Hexalith.Application.Organizations.Helpers;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.InventoryItems.Aggregates;
using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Inventories.PartnerInventoryItems.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Helpers;
using Hexalith.Infrastructure.WebApis.ExternalSystemsEvents.Helpers;
using Hexalith.Infrastructure.WebApis.Helpers;
using Hexalith.Infrastructure.WebApis.InventoriesEvents.Helpers;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers;
using Hexalith.Server.Dynamics365Finance;

IEnumerable<string> aggregateNames = [Customer.GetAggregateName()];
WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    Dynamics365FinanceConstants.ApplicationName,
    "v1",
    (actors) =>
    {
        _ = actors.AddExternalSystemsMapper(Dynamics365FinanceConstants.ApplicationId, aggregateNames);
        _ = actors.AddPartiesProjections(Dynamics365FinanceConstants.ApplicationId);
        actors.RegisterProjectionActor<PartnerInventoryItem>(Dynamics365FinanceConstants.ApplicationId);
        actors.RegisterProjectionActor<InventoryItem>(Dynamics365FinanceConstants.ApplicationId);
    },
    args);

builder.Services
    .AddCustomerProjections(Dynamics365FinanceConstants.ApplicationId)
    .AddInventoryItemsProjections(Dynamics365FinanceConstants.ApplicationId)
    .AddPartnerInventoryItemsProjections(Dynamics365FinanceConstants.ApplicationId)
    .AddExternalSystemsMapperSubscription(Dynamics365FinanceConstants.ApplicationId, aggregateNames)
    .AddDynamics365FinanceCustomers(builder.Configuration, Dynamics365FinanceConstants.ApplicationId)
    .AddDynamics365FinancePartnerInventoryItems(builder.Configuration)
    .AddOrganizations(builder.Configuration)
    .AddScoped<IIntegrationEventHandler<CustomerRegistered>, CustomerRegisteredHandler>()
    .AddScoped<IIntegrationEventHandler<CustomerInformationChanged>, CustomerInformationChangedHandler>()
    .AddScoped<IIntegrationEventProcessor, IntegrationEventProcessor>()
    .AddScoped<IIntegrationEventDispatcher, DependencyInjectionEventDispatcher>();

await builder
    .Build()
    .UseHexalith<Program>(Dynamics365FinanceConstants.ApplicationName)
    .RunAsync()
    .ConfigureAwait(false);
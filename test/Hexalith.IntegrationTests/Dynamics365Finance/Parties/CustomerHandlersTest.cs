// <copyright file="CustomerHandlersTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.IntegrationTests.Dynamics365Finance.Parties;

using System.Net.Http;
using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Application.Organizations.Configurations;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Configuration;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Services;
using Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.TestMocks;
using Hexalith.TestMocks;

using Microsoft.Extensions.Logging;

using Moq;

public class CustomerHandlersTest
{
    [Fact]
    public async Task CheckCanCreateCustomerInDynamics365Finance()
    {
        using HttpClient client = new();
        IDynamics365FinanceClient<CustomerV3> customerV3Service = new Dynamics365FinanceClientBuilder<CustomerV3>()
          .WithValueFromConfiguration<CustomerHandlersTest>()
          .Build(client);
        IDynamics365FinanceClient<CustomerBase> customerBaseService = new Dynamics365FinanceClientBuilder<CustomerBase>()
          .WithValueFromConfiguration<CustomerHandlersTest>()
          .Build(client);
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCustomerService = new Dynamics365FinanceClientBuilder<CustomerExternalSystemCode>()
          .WithValueFromConfiguration<CustomerHandlersTest>()
          .Build(client);
        IDynamics365FinanceClient<RetailStore> storeService = new Dynamics365FinanceClientBuilder<RetailStore>()
          .WithValueFromConfiguration<CustomerHandlersTest>()
          .Build(client);
        IExternalReferenceMapperService mapper = Mock.Of<IExternalReferenceMapperService>();
        Microsoft.Extensions.Options.IOptions<OrganizationSettings> organizationOptions = new OptionsBuilder<OrganizationSettings>()
            .WithValue(new OrganizationSettings { DefaultCompanyId = "frrt", DefaultOriginId = "FinOps", DefaultPartitionId = "TEST" })
            .Build();
        Microsoft.Extensions.Options.IOptions<Dynamics365FinancePartiesSettings> partiesOptions = new OptionsBuilder<Dynamics365FinancePartiesSettings>()
            .WithValue(new Dynamics365FinancePartiesSettings
            {
                Customers = new CustomersSettings
                {
                    SendCustomersToErpEnabled = true,
                    ReceiveCustomersFromErpEnabled = true,
                },
            })
            .Build();
        ILogger<CustomerRegisteredHandler> logger = new LoggerBuilder<CustomerRegisteredHandler>().Build();
        Dynamics365FinanceCustomerService customerService = new(
            customerBaseService,
            customerV3Service,
            externalCustomerService,
            storeService,
            mapper,
            partiesOptions,
            organizationOptions,
            new LoggerBuilder<Dynamics365FinanceCustomerService>().Build());
        CustomerRegisteredHandler handler = new(
            customerService,
            partiesOptions,
            organizationOptions,
            logger);

        CustomerRegistered registered = GetCustomerRegisteredTestEvent();
        IEnumerable<Application.Commands.BaseCommand> commands = await handler.ApplyAsync(registered, CancellationToken.None);
        IEnumerable<CustomerExternalSystemCode> externalCodes = await externalCustomerService.GetAsync(
            new CompanyCustomerExternalCodeFilter(registered.CompanyId, registered.OriginId, registered.Id),
            CancellationToken.None);
        _ = commands.Should().HaveCount(1);
        _ = commands.First().Should().BeOfType<AddExternalSystemReference>();
        _ = externalCodes.Should().NotBeNull();
        _ = externalCodes.Should().HaveCount(1);
        CustomerExternalSystemCode externalCode = externalCodes.First();
        _ = externalCode.Should().NotBeNull();
        string customerId = externalCodes
            .First()
            .CustomerAccountNumber;
        _ = customerId.Should().NotBeEmpty();
        _ = externalCode.ExternalCode.Should().Be(registered.Id);
        _ = externalCode.System.Should().Be(registered.OriginId);
        _ = externalCode.DataAreaId.Should().Be(registered.CompanyId);
        _ = commands.First().Should().BeEquivalentTo(
            new AddExternalSystemReference(
            registered.PartitionId,
            registered.CompanyId,
            organizationOptions.Value.DefaultOriginId,
            registered.AggregateName,
            customerId,
            registered.AggregateId));

        IEnumerable<CustomerV3> customers = await customerV3Service.GetAsync(
            new CustomerByAccountFilter(
                registered.CompanyId,
                customerId),
            CancellationToken.None);
        _ = customers.Should().NotBeNull();
        _ = customers.Should().HaveCount(1);
        _ = customers.First().Should().NotBeNull();
        _ = customers.First().CustomerAccount.Should().Be(customerId);
        CustomerRegistered newRegistered = customers.First().ToCustomerRegisteredEvent(
            registered.PartitionId,
            registered.OriginId,
            registered.Date);
        CustomerV3 expectedCustomer = registered.ToDynamics365FinanceCustomer();
        _ = expectedCustomer
            .Should()
            .BeEquivalentTo(
                customers.First(),
                options =>
                    options.Excluding(p => p.CustomerAccount));
    }

    private static CustomerRegistered GetCustomerRegisteredTestEvent()
    {
        string id = UniqueIdHelper.GenerateUniqueStringId();
        return new(
            "TEST",
            "frrt",
            "MyOrigin",
            id,
            "John Doe",
            PartyType.Person,
            new Contact(
                new Person(
                    "John Doe " + id,
                    "John",
                    "Doe",
                    "Mr",
                    new DateTimeOffset(2001, 04, 12, 0, 0, 0, TimeSpan.Zero),
                    Gender.Female),
                new PostalAddress(
                    "Test",
                    "Test address",
                    "125",
                    "Rue de Madrid",
                    "5684",
                    "75008",
                    "Paris",
                    "FRA",
                    null,
                    null,
                    "FRA",
                    "France",
                    "FR",
                    null,
                    null,
                    null,
                    null),
                "jdoe@mail.com",
                "+33256851255",
                "+33682246555"),
            "Bordeau",
            string.Empty,
            string.Empty,
            "EUR",
            DateTimeOffset.Now);
    }
}
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
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Helpers;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;
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
        IDynamics365FinanceClient<CustomerExternalSystemCode> externalCustomer = new Dynamics365FinanceClientBuilder<CustomerExternalSystemCode>()
          .WithValueFromConfiguration<CustomerHandlersTest>()
          .Build(client);
        IExternalReferenceMapperService mapper = Mock.Of<IExternalReferenceMapperService>();
        Microsoft.Extensions.Options.IOptions<OrganizationSettings> options = new OptionsBuilder<OrganizationSettings>()
            .WithValue(new OrganizationSettings { DefaultCompanyId = "frrt", DefaultOriginId = "FinOps", DefaultPartitionId = "TEST" })
            .Build();
        ILogger<CustomerChangedHandler<CustomerRegistered>> logger = new LoggerBuilder<CustomerChangedHandler<CustomerRegistered>>().Build();
        CustomerRegisteredHandler handler = new(
            customerV3Service,
            externalCustomer,
            mapper,
            options,
            logger);
        CustomerRegistered registered = new(
            "TEST",
            "frrt",
            "MyOrigin",
            UniqueIdHelper.GenerateUniqueStringId(),
            "John Doe",
            new Contact(
                new Person(
                    "John Doe",
                    "John",
                    "Doe",
                    new DateTimeOffset(2001, 04, 12, 0, 0, 0, TimeSpan.Zero),
                    Gender.Female),
                new PostalAddress(
                    "Test",
                    "Test address",
                    "125",
                    "Rue de madrid",
                    "5684",
                    "75008",
                    "Paris",
                    "FRA",
                    null,
                    null,
                    "FRA",
                    "France",
                    "FR"),
                "jdoe@mail.com",
                "+335685125",
                "06822465"),
            null,
            string.Empty,
            DateTimeOffset.Now);
        IEnumerable<Application.Commands.BaseCommand> commands = await handler.ApplyAsync(registered, CancellationToken.None);
        IEnumerable<CustomerExternalSystemCode> externalCodes = await externalCustomer.GetAsync(
            new CustomerExternalCodeFilter(registered.CompanyId, registered.OriginId, registered.Id),
            CancellationToken.None);
        _ = commands.Should().HaveCount(1);
        _ = commands.First().Should().BeOfType<AddExternalSystemReference>();
        _ = externalCodes.Should().NotBeNull();
        _ = externalCodes.Should().HaveCount(1);
        var externalCode = externalCodes.First();
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
            options.Value.DefaultOriginId,
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

        CustomerV3 expectedCustomer = registered.ToDynamics365FinanceCustomer();
        _ = expectedCustomer
            .Should()
            .BeEquivalentTo(
                expectedCustomer,
                options =>
                    options.Excluding(p => p.CustomerAccount));
        _ = customers.First().Should();
    }
}
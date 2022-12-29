// <copyright file="CustomerGroupTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.IntegrationTests.Dynamics365Finance;

using FluentAssertions;

using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

using System.Threading.Tasks;

public class CustomerGroupTest
{
    [Fact]
    public Task Add_inconsitent_data_should_throw_exception()
    {
        Dynamics365FinanceAndOperationsClientBuilder<CustomerGroup> builder = new();
        _ = builder.WithValueFromConfiguration<CustomerGroupTest>();

        IDynamics365FinanceAndOperationsClient<CustomerGroup> service = builder.Build();

        string? company = builder.Settings.Build().Value.Company;
        _ = company.Should().NotBeNullOrWhiteSpace();
        CustomerGroupCreate newGroup = new(
            company!,
            "TEST99",
            clearingPeriodPaymentTermName: null,
            defaultDimensionDisplayValue: null,
            customerAccountNumberSequence: "BADSEQ",
            "TEST",
            "No",
            writeOffReason: null,
            paymentTermId: "BADPAY",
            taxGroupId: "BADTAX",
            "No");
        Func<Task> act = () => service.PostAsync(newGroup, CancellationToken.None);
        return act.Should().ThrowAsync<Dynamics365FinancePostException<CustomerGroup, CustomerGroupCreate>>();
    }

    [Fact]
    public async Task Check_can_add_new_group()
    {
        Dynamics365FinanceAndOperationsClientBuilder<CustomerGroup> builder = new();
        _ = builder.WithValueFromConfiguration<CustomerGroupTest>();

        IDynamics365FinanceAndOperationsClient<CustomerGroup> service = builder.Build();

        string? company = builder.Settings.Build().Value.Company;
        _ = company.Should().NotBeNullOrWhiteSpace();
        CustomerGroupCreate newGroup = new(
            company!,
            "TEST992",
            clearingPeriodPaymentTermName: null,
            defaultDimensionDisplayValue: null,
            customerAccountNumberSequence: null,
            "TEST",
            "No",
            writeOffReason: null,
            paymentTermId: null,
            taxGroupId: null,
            "No");
        CustomerGroup group = await service.PostAsync(newGroup, CancellationToken.None);
        _ = group.Should().NotBeNull();
        _ = group.CustomerGroupId.Should().Be(newGroup.CustomerGroupId);
        _ = group.Description.Should().Be(newGroup.Description);
    }

    [Fact]
    public async Task Check_can_update_group_description()
    {
        Dynamics365FinanceAndOperationsClientBuilder<CustomerGroup> builder = new();
        _ = builder.WithValueFromConfiguration<CustomerGroupTest>();

        IDynamics365FinanceAndOperationsClient<CustomerGroup> service = builder.Build();

        string? company = builder.Settings.Build().Value.Company;
        _ = company.Should().NotBeNullOrWhiteSpace();
        const string id = "TST990";
        string description = "HELLO TEST 990 Updated " + UniqueIdHelper.GenerateDateTimeId();
        CustomerGroupKey key = new(CustomerGroupId: id);
        if (!await service.ExistsAsync(key, CancellationToken.None))
        {
            CustomerGroupCreate newGroup = new(
               company!,
               id,
               clearingPeriodPaymentTermName: null,
               defaultDimensionDisplayValue: null,
               customerAccountNumberSequence: null,
               "TEST",
               "No",
               writeOffReason: null,
               paymentTermId: null,
               taxGroupId: null,
               "No");
            _ = await service.PostAsync(newGroup, CancellationToken.None);
        }

        await service.PatchAsync(key, new CustomerGroupUpdate { Description = description }, CancellationToken.None);
        CustomerGroup group = await service.GetSingleAsync(key, CancellationToken.None);
        _ = group.Should().NotBeNull();
        _ = group.Description.Should().Be(description);
    }
}

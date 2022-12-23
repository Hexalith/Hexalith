// <copyright file="CustomerGroupTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.IntegrationTests.Dynamics365Finance;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Services.CustomerGroups;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;
using Hexalith.TestMocks;

using System.Threading.Tasks;

public class CustomerGroupTest
{
    [Fact]
    public Task Add_inconsitent_data_should_throw_exception()
    {
        Dynamics365FinanceAndOperationsClientBuilder<CustomerGroup> builder = new();
        _ = builder.WithValueFromConfiguration<CustomerGroupTest>();

        CustomerGroupService service = new(
           builder.Build(),
           new LoggerBuilder<CustomerGroupService>().Build());
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
        Func<Task> act = () => service.InsertAsync(newGroup, CancellationToken.None);
        return act.Should().ThrowAsync<Dynamics365FinanceInsertException<CustomerGroup, CustomerGroupCreate>>();
    }

    [Fact]
    public async Task Check_can_add_new_group()
    {
        Dynamics365FinanceAndOperationsClientBuilder<CustomerGroup> builder = new();
        _ = builder.WithValueFromConfiguration<CustomerGroupTest>();

        CustomerGroupService service = new(
           builder.Build(),
           new LoggerBuilder<CustomerGroupService>().Build());

        string? company = builder.Settings.Build().Value.Company;
        _ = company.Should().NotBeNullOrWhiteSpace();
        CustomerGroupCreate newGroup = new(
            company!,
            "TEST990",
            clearingPeriodPaymentTermName: null,
            defaultDimensionDisplayValue: null,
            customerAccountNumberSequence: null,
            "TEST",
            "No",
            writeOffReason: null,
            paymentTermId: null,
            taxGroupId: null,
            "No");
        CustomerGroup group = await service.InsertAsync(newGroup, CancellationToken.None);
        _ = group.Should().NotBeNull();
        _ = group.CustomerGroupId.Should().Be(newGroup.CustomerGroupId);
        _ = group.Description.Should().Be(newGroup.Description);
    }
}

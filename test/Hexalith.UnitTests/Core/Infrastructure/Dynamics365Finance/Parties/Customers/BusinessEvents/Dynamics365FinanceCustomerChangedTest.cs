// <copyright file="Dynamics365FinanceCustomerChangedTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using FluentAssertions;

using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

public class Dynamics365FinanceCustomerChangedTest
{
    [Fact]
    public void Dynamics365FinanceCustomerChangedSerializationAndDeserializationShouldHaveSameResult()
    {
        Dynamics365FinanceCustomerChanged be = new()
        {
            Account = "Cust123456",
            BusinessEventId = "MyEvent",
            CommissionSalesGroupId = "GRP1",
            ControlNumber = 123456789022L,
            EventId = "1234567890",
            EventTime = DateTime.UtcNow,
            EventTimeIso8601 = DateTime.Now,
            BusinessEventLegalEntity = "FRRT",
            InitiatingUserAzureActiveDirectoryObjectId = "4251234567890",
            InterCompanyDirectDelivery = "Yes",
            MajorVersion = 1,
            MinorVersion = 2,
            Name = "My Name",
            PartitionId = "PART1",
            WarehouseId = "WH1",
            Contact = new Contact(
                new Person(
                    "John Doe",
                    "John",
                    "Doe",
                    DateTimeOffset.Now.Add(TimeSpan.FromDays(-50000d)),
                    Gender.Male),
                new PostalAddress(
                    "Primary",
                    "Primary address",
                    "31",
                    "Coventry street",
                    "25669",
                    "67008",
                    "London",
                    "GB",
                    "LDN",
                    "London City",
                    "GBR",
                    "Great britain",
                    "GB"),
                "jdoe@ms.com",
                "+33321563",
                "+33652952"),
            ExternalReferences =
            [
                new ExternalReference("MyRef1", "MyRefValue 1"),
                new ExternalReference("MyRef2", "MyRefValue 2"),
                new ExternalReference("MyRef3", "MyRefValue 3"),
                new ExternalReference("MyRef4", "MyRefValue 4")
            ],
        };
        _ = be.Should().BeDataContractSerializable();
    }
}
// <copyright file="CustomerV3CreateTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

using Hexalith.TestMocks;

public class CustomerV3CreateTest : SerializationTestBase
{
    public override object ToSerializeObject() => DummyDynamicsCustomersHelper.DummyCustomerV3Create;
}
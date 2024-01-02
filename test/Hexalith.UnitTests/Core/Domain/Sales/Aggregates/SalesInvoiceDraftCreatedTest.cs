// <copyright file="SalesInvoiceDraftCreatedTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Sales.Aggregates;

using Hexalith.TestMocks;

public class SalesInvoiceStateTest : SerializationTestBase
{
    public override object ToSerializeObject() => DummySalesDomainHelper.DummySalesInvoiceState();
}
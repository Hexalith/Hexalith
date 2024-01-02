// <copyright file="SalesLineOriginTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Sales.Events;

using Hexalith.Domain.Events;
using Hexalith.TestMocks;

public class SalesInvoiceDraftCreatedTest : SerializationTestBase
{
    public override object ToSerializeObject() => new SalesInvoiceDraftCreated("TST", "CPY", "ORG", "INV456987", "Cust456");
}
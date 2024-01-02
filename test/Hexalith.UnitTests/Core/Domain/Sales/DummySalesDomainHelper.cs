// <copyright file="DummySalesDomainHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Sales;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Entities;
using Hexalith.Domain.Events;
using Hexalith.Domain.ValueObjets;

public static class DummySalesDomainHelper
{
    public static SalesInvoiceDraftCreated DummySalesInvoiceDraftCreated()
        => new("TST", "CPY", "ORG", "INV456987", "Cust456");

    public static SalesInvoiceIssued DummySalesInvoiceIssued()
        => new("TST", "CPY", "ORG", "INV123456", DateTimeOffset.Now);

    public static SalesInvoiceLineState DummySalesInvoiceLineState()
        => new("Line1", DummySalesLineItem(), DummySalesLineOrigin(), new List<SalesLineTax>() { DummySalesLineTax(), DummySalesLineTax2() });

    public static SalesInvoiceLineState DummySalesInvoiceLineState2()
        => new("Line2", DummySalesLineItem2(), DummySalesLineOrigin(), new List<SalesLineTax>() { DummySalesLineTax2() });

    public static SalesInvoiceLineState DummySalesInvoiceLineState3()
        => new("Line3", DummySalesLineItem3(), DummySalesLineOrigin(), new List<SalesLineTax>());

    public static SalesInvoiceState DummySalesInvoiceState()
                    => new();

    public static SalesLineItem DummySalesLineItem()
        => new("MyItem123", 101.20m, "Kg", 50.25m);

    public static SalesLineItem DummySalesLineItem2()
        => new("MyItem456", 1.25m, "L", 60.10m);

    public static SalesLineItem DummySalesLineItem3()
        => new("MyItem789", 77.25m, "M3", 102.45m);

    public static SalesLineOrigin DummySalesLineOrigin()
        => new("TheLocation123", "Vendor546");

    public static SalesLineTax DummySalesLineTax()
        => new("T55", "Tax 5.5%", 50.25m);

    public static SalesLineTax DummySalesLineTax2()
        => new("T20", "Tax 20%", 50.25m);
}
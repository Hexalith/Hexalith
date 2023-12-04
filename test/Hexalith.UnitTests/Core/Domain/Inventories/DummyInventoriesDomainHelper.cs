// <copyright file="DummyInventoriesDomainHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Inventories;

using Hexalith.Domain.ValueObjets;

public static class DummyInventoriesDomainHelper
{
    public static ProductIdentifier DummyProductIdentifier()
        => new()
        {
            Barcode = "1234567890123",
            ColorId = "COL1",
            ConfigurationId = "CONF1",
            ItemId = "ITEM1",
            Quantity = 10.25m,
            SizeId = "SIZE1",
            StyleId = "STYLE1",
            SystemId = "SYS1",
            UnitId = "UNIT1",
            VersionId = "125",
        };
}
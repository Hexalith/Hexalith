// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 09-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-25-2023
// ***********************************************************************
// <copyright file="WarehouseOnHandFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.InventoryOnHand;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class WarehouseOnHandFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="IEquatable{WarehouseOnHandFilter}" />.
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="IEquatable{WarehouseOnHandFilter}" />
[DataContract]
public record WarehouseOnHandFilter
(
    string DataAreaId,
    [property: DataMember(Order = 1)] string InventoryWarehouseId)
    : PerCompanyFilter(DataAreaId)
{
}
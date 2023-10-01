// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 09-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-25-2023
// ***********************************************************************
// <copyright file="WarehouseOnHandFilter.cs" company="Fiveforty S.A.S.">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class WarehouseOnHandFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.WarehouseOnHandFilter}" />
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.WarehouseOnHandFilter}" />
public record WarehouseOnHandFilter
(
    string DataAreaId,
    string InventoryWarehouseId)
    : PerCompanyFilter(DataAreaId)
{
}
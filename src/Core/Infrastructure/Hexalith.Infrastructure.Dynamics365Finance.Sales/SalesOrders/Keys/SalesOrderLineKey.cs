// <copyright file="SalesOrderLineKey.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Keys;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderLineKey.
/// Implements the <see cref="PerCompanyPrimaryKey" />
/// Implements the <see cref="IPerCompanyPrimaryKey" />
/// Implements the <see cref="IPrimaryKey" />
/// Implements the <see cref="IEquatable{PerCompanyPrimaryKey}" />
/// Implements the <see cref="IEquatable{SalesOrderLineKey}" />.</summary>
[DataContract]
public record SalesOrderLineKey(
    string? DataAreaId,
    string? SalesOrderNumber,
    decimal LineNumber)
    : PerCompanyPrimaryKey(DataAreaId)
{
}
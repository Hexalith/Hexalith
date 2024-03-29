// <copyright file="SalesOrderHeaderAdditionalUpdateOrigin.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderHeaderAdditionalUpdate.
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="SalesOrderHeaderAdditionalUpdateOrigin"/> class.</remarks>
/// <param name="salesOriginId">The sales origin.</param>
[DataContract]
[method: JsonConstructor]
public class SalesOrderHeaderAdditionalUpdateOrigin(string? salesOriginId)
{
    /// <summary>Gets the sales order origin code.</summary>
    /// <value>The sales order origin code.</value>
    public string? SalesOriginId { get; } = salesOriginId;
}
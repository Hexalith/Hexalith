// <copyright file="SalesOrderHeaderAdditionalUpdate.cs" company="Fiveforty SAS Paris France">
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
/// <remarks>Initializes a new instance of the <see cref="SalesOrderHeaderAdditionalUpdate"/> class.</remarks>
/// <param name="phone">The phone.</param>
/// <param name="deadline">The deadline.</param>
[DataContract]
[method: JsonConstructor]
public class SalesOrderHeaderAdditionalUpdate(
    string? phone,
    DateTimeOffset? deadline)
{
    /// <summary>Gets the deadline.</summary>
    /// <value>The deadline.</value>
    public DateTimeOffset? Deadline { get; } = deadline;

    /// <summary>Gets the phone.</summary>
    /// <value>The phone.</value>
    public string? Phone { get; } = phone;
}
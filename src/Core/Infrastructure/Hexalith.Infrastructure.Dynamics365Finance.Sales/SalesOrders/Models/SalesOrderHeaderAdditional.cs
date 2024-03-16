// <copyright file="SalesOrderHeaderAdditional.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderHeaderAdditional.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderHeaderAdditional}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderHeaderAdditional}" />
[DataContract]
public record SalesOrderHeaderAdditional(
    string Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] string SalesOrderNumber,
    [property: DataMember(Order = 4)] string Phone,
    [property: DataMember(Order = 5)] DateTimeOffset? Deadline,
    [property: DataMember(Order = 6)] string SalesOriginId)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "FFYSalesOrderHeaderAdditionals";
}
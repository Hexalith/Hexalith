// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 10-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-08-2023
// ***********************************************************************
// <copyright file="SalesOrderLineAdditional.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderLineAdditional.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderLineAdditional}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderLineAdditional}" />
[DataContract]
public record SalesOrderLineAdditional(
    string Etag,
    string DataAreaId,
    [property: DataMember(Order = 3)] long SalesLineRecId,
    [property: DataMember(Order = 4)] string SalesId,
    [property: DataMember(Order = 5)] decimal LineNum,
    [property: DataMember(Order = 6)] string DeliveryType)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "FFYSalesLinesAdditional";
}
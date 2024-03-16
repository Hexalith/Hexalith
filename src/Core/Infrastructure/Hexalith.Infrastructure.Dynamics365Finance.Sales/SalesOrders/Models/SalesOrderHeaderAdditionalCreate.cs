// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 12-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-04-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderAdditionalCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderHeaderAdditionalCreate.
/// Implements the <see cref="IEquatable{SalesOrderHeaderAdditionalCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderHeaderAdditionalCreate}" />
[DataContract]
public record SalesOrderHeaderAdditionalCreate(
    [property: JsonPropertyName("dataAreaId")] string DataAreaId,
    [property: DataMember(Order = 2)] string SalesOrderNumber,
    [property: DataMember(Order = 3)] DateTimeOffset? DeadLine,
    [property: DataMember(Order = 4)] string? Phone)
{
}
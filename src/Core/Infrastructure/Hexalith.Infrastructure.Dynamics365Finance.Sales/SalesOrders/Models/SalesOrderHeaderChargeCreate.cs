// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 02-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-12-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderChargeCreate.cs" company="Fiveforty SAS Paris France">
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
/// Class SalesOrderHeaderChargeCreate.
/// Implements the <see cref="IEquatable{SalesOrderHeaderChargeCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderHeaderChargeCreate}" />
[DataContract]
public record SalesOrderHeaderChargeCreate(
        [property: DataMember(Order = 1)][property: JsonPropertyName("dataAreaId")] string DataAreaId,
        [property: DataMember(Order = 2)] string SalesOrderNumber,
        [property: DataMember(Order = 3)] decimal ChargeLineNumber,
        [property: DataMember(Order = 4)] string ChargeDescription,
        [property: DataMember(Order = 5)] string SalesChargeCode,
        [property: DataMember(Order = 6)] decimal FixedChargeAmount)
{
}
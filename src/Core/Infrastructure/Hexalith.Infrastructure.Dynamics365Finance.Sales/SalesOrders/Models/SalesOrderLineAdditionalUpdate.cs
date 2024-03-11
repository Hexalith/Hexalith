// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 10-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-09-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderAdditionalUpdate - Copy.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderLineAdditionalUpdate.
/// </summary>
[DataContract]
[method: JsonConstructor]
public class SalesOrderLineAdditionalUpdate(
    string? deliveryType)
{
    /// <summary>
    /// Gets or sets the phone.
    /// </summary>
    /// <value>The phone.</value>
    public string? DeliveryType { get; set; } = deliveryType;
}
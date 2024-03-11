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
public record SalesOrderHeaderAdditionalCreate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderHeaderAdditionalCreate"/> class.
    /// </summary>
    /// <param name="dataAreaId">The data area identifier.</param>
    /// <param name="salesOrderNumber">The sales order number.</param>
    /// <param name="deadLine">The dead line.</param>
    /// <param name="phone">The phone.</param>
    public SalesOrderHeaderAdditionalCreate(
        string dataAreaId,
        string salesOrderNumber,
        DateTimeOffset? deadLine,
        string? phone)
    {
        DataAreaId = dataAreaId;
        SalesOrderNumber = salesOrderNumber;
        DeadLine = deadLine;
        Phone = phone;
    }

    /// <summary>
    /// Gets the data area identifier.
    /// </summary>
    /// <value>The data area identifier.</value>
    [JsonPropertyName("dataAreaId")]
    public string DataAreaId { get; }

    /// <summary>
    /// Gets the sales order number.
    /// </summary>
    /// <value>The sales order number.</value>
    public string SalesOrderNumber { get; }

    /// <summary>
    /// Gets the dead line.
    /// </summary>
    /// <value>The dead line.</value>
    public DateTimeOffset? DeadLine { get; }

    /// <summary>
    /// Gets the phone.
    /// </summary>
    /// <value>The phone.</value>
    public string? Phone { get; }
}
// <copyright file="SalesOrderExternalCodeCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

// namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

// using System.Runtime.Serialization;
// using System.Text.Json.Serialization;

// [DataContract]
// public record SalesOrderExternalCodeCreate
// {
//    /// <summary>
//    /// Initializes a new instance of the <see cref="SalesOrderExternalCodeCreate"/> class.
//    /// </summary>
//    /// <param name="dataAreaId"></param>
//    /// <param name="sourceId"></param>
//    /// <param name="salesOrderNumber"></param>
//    /// <param name="externalId"></param>
//    public SalesOrderExternalCodeCreate(
//        string dataAreaId,
//        string sourceId,
//        string salesOrderNumber,
//        string externalId)
//    {
//        DataAreaId = dataAreaId;
//        SourceId = sourceId;
//        SalesOrderNumber = salesOrderNumber;
//        ExternalId = externalId;
//    }

// [JsonPropertyName("dataAreaId")]
//    public string DataAreaId { get; }

// public string SourceId { get; }

// public string SalesOrderNumber { get; }

// public string ExternalId { get; }
// }
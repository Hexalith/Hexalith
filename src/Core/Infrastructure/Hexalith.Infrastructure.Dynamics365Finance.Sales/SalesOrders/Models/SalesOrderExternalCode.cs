// <copyright file="SalesOrderExternalCode.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

[DataContract]
public record SalesOrderExternalCode(string Etag, string DataAreaId, string SourceId, string SalesOrderNumber, string ExternalId)
    : ODataElement(Etag, DataAreaId)
{
}
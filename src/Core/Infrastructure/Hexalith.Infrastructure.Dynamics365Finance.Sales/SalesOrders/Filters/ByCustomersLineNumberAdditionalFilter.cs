// <copyright file="ByCustomersLineNumberAdditionalFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Filters;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

[DataContract]
public record ByCustomersLineNumberAdditionalFilter(
    string DataAreaId,
    [property: DataMember(Order = 2)] string SalesId,
    [property: DataMember(Order = 3)] decimal LineNum)
    : PerCompanyFilter(DataAreaId)
{
}
// <copyright file="CustomerByAccountFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

[DataContract]
public record CustomerByAccountFilter
(
    string DataAreaId,
    [property: DataMember(Order = 2)] string CustomerAccount)
    : PerCompanyFilter(DataAreaId)
{
}
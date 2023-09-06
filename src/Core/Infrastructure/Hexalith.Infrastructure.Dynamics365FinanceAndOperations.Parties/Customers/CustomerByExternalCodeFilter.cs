// <copyright file="CustomerByExternalCodeFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Parties.Customers;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

public record CustomerExternalCodeFilter
(
    string DataAreaId,
    string CustomerExternalCodeClassId,
    string ExternalCode)
    : PerCompanyFilter(DataAreaId)
{
}
// <copyright file="SalesInvoiceByAccountFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Entities;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

public record SalesInvoiceByNameFilter
(
    string DataAreaId,
    string Name)
    : PerCompanyFilter(DataAreaId)
{
}
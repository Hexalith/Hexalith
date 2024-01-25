// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Sales
// Author           : Jérôme Piquot
// Created          : 09-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-07-2023
// ***********************************************************************
// <copyright file="SalesInvoiceExternalCodeFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesInvoices.Filters;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesInvoiceExternalCodeFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="IPerCompanyFilter" />
/// Implements the <see cref="IFilter" />
/// Implements the <see cref="IEquatable{PerCompanyFilter}" />
/// Implements the <see cref="IEquatable{SalesInvoiceExternalCodeFilter}" />.
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="IPerCompanyFilter" />
/// <seealso cref="IFilter" />
/// <seealso cref="IEquatable{PerCompanyFilter}" />
/// <seealso cref="IEquatable{SalesInvoiceExternalCodeFilter}" />
public record SalesInvoiceExternalCodeFilter
(
    string DataAreaId,
    string System,
    string ExternalCode)
    : PerCompanyFilter(DataAreaId)
{
}
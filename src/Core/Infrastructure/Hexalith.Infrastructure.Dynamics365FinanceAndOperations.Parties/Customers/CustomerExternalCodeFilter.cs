// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Parties
// Author           : Jérôme Piquot
// Created          : 09-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-07-2023
// ***********************************************************************
// <copyright file="CustomerExternalCodeFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Parties.Customers;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

/// <summary>
/// Class CustomerExternalCodeFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="IPerCompanyFilter" />
/// Implements the <see cref="IFilter" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models.PerCompanyFilter}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Parties.Customers.CustomerExternalCodeFilter}" />.
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="IPerCompanyFilter" />
/// <seealso cref="IFilter" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models.PerCompanyFilter}" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Parties.Customers.CustomerExternalCodeFilter}" />
public record CustomerExternalCodeFilter
(
    string DataAreaId,
    string CustomerExternalCodeClassId,
    string ExternalCode)
    : PerCompanyFilter(DataAreaId)
{
}
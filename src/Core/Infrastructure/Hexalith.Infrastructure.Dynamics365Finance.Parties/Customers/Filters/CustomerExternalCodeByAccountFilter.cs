// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 09-06-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-06-2023
// ***********************************************************************
// <copyright file="CustomerExternalCodeByAccountFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Filters;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class CustomerExternalCodeByAccountFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="IPerCompanyFilter" />
/// Implements the <see cref="IFilter" />
/// Implements the <see cref="IEquatable{PerCompanyFilter}" />
/// Implements the <see cref="IEquatable{CustomerExternalCodeByAccountFilter}" />.
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="IPerCompanyFilter" />
/// <seealso cref="IFilter" />
/// <seealso cref="IEquatable{PerCompanyFilter}" />
/// <seealso cref="IEquatable{CustomerExternalCodeByAccountFilter}" />
[DataContract]
public record CustomerExternalCodeByAccountFilter
(
    string DataAreaId,
    [property: DataMember(Order = 2)] string System,
    [property: DataMember(Order = 3)] string CustomerAccountNumber)
    : PerCompanyFilter(DataAreaId)
{
}
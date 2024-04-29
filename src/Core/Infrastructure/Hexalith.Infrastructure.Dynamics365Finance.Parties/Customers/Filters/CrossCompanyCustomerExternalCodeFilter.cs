// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 09-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-07-2023
// ***********************************************************************
// <copyright file="CrossCompanyCustomerExternalCodeFilter.cs" company="Fiveforty SAS Paris France">
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
/// Class CustomerExternalCodeFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="IPerCompanyFilter" />
/// Implements the <see cref="IFilter" />
/// Implements the <see cref="IEquatable{PerCompanyFilter}" />
/// Implements the <see cref="IEquatable{CustomerExternalCodeFilter}" />.
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="IPerCompanyFilter" />
/// <seealso cref="IFilter" />
/// <seealso cref="IEquatable{PerCompanyFilter}" />
/// <seealso cref="IEquatable{CustomerExternalCodeFilter}" />
[DataContract]
public record CrossCompanyCustomerExternalCodeFilter
(
    [property: DataMember(Order = 2)] string System,
    [property: DataMember(Order = 3)] string ExternalCode)
    : ICommonFilter
{
}
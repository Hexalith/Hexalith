// <copyright file="CustomerGroupKey.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class CustomerGroupKey.
/// Implements the <see cref="PerCompanyPrimaryKey" />
/// Implements the <see cref="IPerCompanyPrimaryKey" />
/// Implements the <see cref="IPrimaryKey" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Models.PerCompanyPrimaryKey}" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups.CustomerGroupKey}" />.
/// </summary>
/// <seealso cref="PerCompanyPrimaryKey" />
/// <seealso cref="IPerCompanyPrimaryKey" />
/// <seealso cref="IPrimaryKey" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Models.PerCompanyPrimaryKey}" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Services.CustomerGroups.CustomerGroupKey}" />
[DataContract]
public record CustomerGroupKey(
      string? DataAreaId = null,
      [property: DataMember(Order = 1)] string? CustomerGroupId = null) : PerCompanyPrimaryKey(DataAreaId)
{
}
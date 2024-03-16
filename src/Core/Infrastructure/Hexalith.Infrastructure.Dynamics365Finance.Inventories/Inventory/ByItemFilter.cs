// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 03-09-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-09-2023
// ***********************************************************************
// <copyright file="ByItemFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Inventories.Inventory;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class ByItemFilter.
/// Implements the <see cref="PerCompanyFilter" />
/// Implements the <see cref="IPerCompanyFilter" />
/// Implements the <see cref="IFilter" />
/// Implements the <see cref="IEquatable{PerCompanyFilter}" />
/// Implements the <see cref="IEquatable{ByItemFilter}" />.
/// </summary>
/// <seealso cref="PerCompanyFilter" />
/// <seealso cref="IPerCompanyFilter" />
/// <seealso cref="IFilter" />
/// <seealso cref="IEquatable{PerCompanyFilter}" />
/// <seealso cref="IEquatable{ByItemFilter}" />
[DataContract]
public record ByItemFilter
(
    string DataAreaId,
    [property: DataMember(Order = 2)] string ItemNumber,
    [property: DataMember(Order = 3)] string? ProductStyleId,
    [property: DataMember(Order = 4)] string? ProductColorId,
    [property: DataMember(Order = 5)] string? ProductSizeId)
    : PerCompanyFilter(DataAreaId)
{
}
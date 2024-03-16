// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Retail
// Author           : Jérôme Piquot
// Created          : 12-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="RetailStore.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class RetailStore.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Models.ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities.RetailStore}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Models.ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities.RetailStore}" />
[DataContract]
public record RetailStore
(
    [property: DataMember(Order = 2)] string? RetailChannelId = null,
    [property: DataMember(Order = 3)] string? DefaultCustomerAccount = null,
    [property: DataMember(Order = 4)] string? DefaultCustomerLegalEntity = null,
    [property: DataMember(Order = 5)] string? WareHouseId = null,
    [property: DataMember(Order = 6)] string? WarehouseLegalEntity = null,
    [property: DataMember(Order = 7)] string? Currency = null)
: ODataCommon(Etag: null), IODataCommon
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "RetailStores";
}
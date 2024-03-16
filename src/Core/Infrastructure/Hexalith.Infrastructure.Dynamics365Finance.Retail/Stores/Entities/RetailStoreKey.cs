// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Retail
// Author           : Jérôme Piquot
// Created          : 12-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-13-2023
// ***********************************************************************
// <copyright file="RetailStoreKey.cs" company="Fiveforty SAS Paris France">
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
/// Class RetailStoreAccountKey.
/// Implements the <see cref="ICommonPrimaryKey" />
/// Implements the <see cref="IPrimaryKey" />
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities.RetailStoreKey}" />.
/// </summary>
/// <seealso cref="ICommonPrimaryKey" />
/// <seealso cref="IPrimaryKey" />
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.Dynamics365Finance.Retail.Stores.Entities.RetailStoreKey}" />
[DataContract]
public record RetailStoreKey([property: DataMember(Order = 1)] string RetailChannelId)
    : ICommonPrimaryKey
{
}
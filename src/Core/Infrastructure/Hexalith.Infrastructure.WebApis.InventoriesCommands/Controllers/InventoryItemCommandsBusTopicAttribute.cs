// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.ExternalSystemsCommands
// Author           : Jérôme Piquot
// Created          : 11-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryItemCommandsBusTopicAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.InventoriesCommands.Controllers;

using Hexalith.Domain.InventoryItems.Aggregates;
using Hexalith.Infrastructure.WebApis.Buses;

/// <summary>
/// Class InventoryItemCommandsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="CommandBusTopicAttribute" />.
/// </summary>
/// <seealso cref="CommandBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class InventoryItemCommandsBusTopicAttribute : CommandBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryItemCommandsBusTopicAttribute" /> class.
    /// </summary>
    public InventoryItemCommandsBusTopicAttribute()
        : base(InventoryItem.GetAggregateName())
    {
    }
}
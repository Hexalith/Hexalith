// <copyright file="PartnerInventoryItemCommandsBusTopicAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.InventoriesCommands.Controllers;

using Hexalith.Domain.PartnerInventoryItems.Aggregates;
using Hexalith.Infrastructure.WebApis.Buses;

/// <summary>
/// Class PartnerInventoryItemCommandsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="CommandBusTopicAttribute" />.
/// </summary>
/// <seealso cref="CommandBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class PartnerInventoryItemCommandsBusTopicAttribute : CommandBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartnerInventoryItemCommandsBusTopicAttribute"/> class.
    /// </summary>
    public PartnerInventoryItemCommandsBusTopicAttribute()
        : base(PartnerInventoryItem.GetAggregateName())
    {
    }
}
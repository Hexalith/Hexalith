// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="InventoryUnitConversionCommandsBusTopicAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.InventoriesCommands.Controllers;

using Hexalith.Domain.InventoryUnitConversions.Aggregates;
using Hexalith.Infrastructure.WebApis.Buses;

/// <summary>
/// Class InventoryUnitConversionCommandsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="CommandBusTopicAttribute" />.
/// </summary>
/// <seealso cref="CommandBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class InventoryUnitConversionCommandsBusTopicAttribute : CommandBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryUnitConversionCommandsBusTopicAttribute" /> class.
    /// </summary>
    public InventoryUnitConversionCommandsBusTopicAttribute()
        : base(InventoryUnitConversion.GetAggregateName())
    {
    }
}
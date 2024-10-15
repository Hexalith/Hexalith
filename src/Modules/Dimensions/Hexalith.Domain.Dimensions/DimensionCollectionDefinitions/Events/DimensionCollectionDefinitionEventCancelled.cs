// <copyright file="DimensionCollectionDefinitionEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event indicating that a DimensionCollectionDefinition event has been cancelled.
/// </summary>
[PolymorphicSerialization]
public partial record DimensionCollectionDefinitionEventCancelled(
    /// <summary>
    /// Gets the original DimensionCollectionDefinition event that was cancelled.
    /// </summary>
    DimensionCollectionDefinitionEvent Event,

    /// <summary>
    /// Gets the reason for cancelling the event.
    /// </summary>
    string Reason) : DimensionCollectionDefinitionEvent(Event.Id);

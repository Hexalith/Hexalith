// <copyright file="DimensionCollectionDefinitionEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Dimensions.DimensionCollectionDefinitions.Events;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event indicating that a DimensionCollectionDefinition event has been cancelled.
/// </summary>
/// <param name="Event">The event that has been cancelled.</param>
/// <param name="Reason">The reason why the event has been cancelled.</param>
[PolymorphicSerialization]
public partial record DimensionCollectionDefinitionEventCancelled(
    DimensionCollectionDefinitionEvent Event,
    string Reason) : DimensionCollectionDefinitionEvent(Event.Id);
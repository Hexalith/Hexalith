// <copyright file="DomainEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Runtime.Serialization;

using Hexalith.Application.States;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event that indicates a domain event has been cancelled.
/// </summary>
/// <param name="Reason">The reason why the domain event was cancelled.</param>
/// <param name="Event">The domain event that was cancelled.</param>
[PolymorphicSerialization]
public partial record DomainEventCancelled(
    [property: DataMember(Order = 1)] string Reason,
    [property: DataMember(Order = 2)] MessageState Event)
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Event.Metadata.Message.Aggregate.Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public string AggregateName => Event.Metadata.Message.Aggregate.Name;
}
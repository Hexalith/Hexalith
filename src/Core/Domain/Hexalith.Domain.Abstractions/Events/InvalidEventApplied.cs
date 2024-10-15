// <copyright file="InvalidEventApplied.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an invalid event that was applied to an aggregate.
/// </summary>
/// <param name="AggregateName">The name of the aggregate to which the invalid event was applied.</param>
/// <param name="AggregateId">The unique identifier of the aggregate.</param>
/// <param name="EventType">The type of the event that was invalid.</param>
/// <param name="EventContent">The serialized JSON content of the invalid event.</param>
/// <param name="Reason">The reason why the event was considered invalid.</param>
[PolymorphicSerialization]
public record InvalidEventApplied(string AggregateName, string AggregateId, string EventType, string EventContent, string Reason);
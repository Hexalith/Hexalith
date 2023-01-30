// <copyright file="AggregateActorState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprAggregateActor;

using System.Runtime.Serialization;

/// <summary>
/// Class AggregateActorState.
/// Implements the <see cref="System.IEquatable{Hexalith.Infrastructure.DaprAggregateActor.AggregateActorState}" />.
/// </summary>
/// <seealso cref="System.IEquatable{Hexalith.Infrastructure.DaprAggregateActor.AggregateActorState}" />
[DataContract]
public record AggregateActorState(
    long CommandStreamVersion,
    long EventStreamVersion,
    long LastCommandDone,
    long LastEventPublished)
{
}
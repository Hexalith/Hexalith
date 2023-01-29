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
    long NextCommandToDo,
    long NextEventToPublish)
{
    /// <summary>
    /// Increments the command version.
    /// </summary>
    /// <returns>AggregateActorState.</returns>
    public AggregateActorState IncrementCommandVersion()
    {
        return new AggregateActorState(
                CommandStreamVersion + 1,
                EventStreamVersion,
                NextCommandToDo,
                NextEventToPublish);
    }

    /// <summary>
    /// Increments the event version.
    /// </summary>
    /// <returns>AggregateActorState.</returns>
    public AggregateActorState IncrementEventVersion()
    {
        return new AggregateActorState(
                CommandStreamVersion,
                EventStreamVersion + 1,
                NextCommandToDo,
                NextEventToPublish);
    }

    /// <summary>
    /// Increments the next command to do.
    /// </summary>
    /// <returns>AggregateActorState.</returns>
    public AggregateActorState IncrementNextCommandToDo()
    {
        return new AggregateActorState(
                CommandStreamVersion,
                EventStreamVersion,
                NextCommandToDo + 1,
                NextEventToPublish);
    }

    /// <summary>
    /// Increments the next event to publish.
    /// </summary>
    /// <returns>AggregateActorState.</returns>
    public AggregateActorState IncrementNextEventToPublish()
    {
        return new AggregateActorState(
                CommandStreamVersion,
                EventStreamVersion,
                NextCommandToDo,
                NextEventToPublish + 1);
    }

    /// <summary>
    /// Withes the event version.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <returns>AggregateActorState.</returns>
    public AggregateActorState WithEventVersion(long version)
    {
        return new AggregateActorState(this) { CommandStreamVersion = version };
    }
}
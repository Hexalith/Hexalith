// <copyright file="AggregateState.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Runtime.Serialization;

/// <summary>
/// Class AggregateActorState.
/// Implements the <see cref="IEquatable{AggregateActorState}" />.
/// </summary>
/// <seealso cref="IEquatable{AggregateActorState}" />
[DataContract]
public record AggregateState(
        [property: DataMember] long CommandStreamVersion,
        [property: DataMember] long EventStreamVersion,
        [property: DataMember] long LastCommandDone,
        [property: DataMember] long LastEventPublished)
{
}
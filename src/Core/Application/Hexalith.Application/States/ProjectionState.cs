// <copyright file="ProjectionState.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Runtime.Serialization;

/// <summary>
/// Class ProjectionActorState.
/// Implements the <see cref="IEquatable{ProjectionActorState}" />.
/// </summary>
/// <seealso cref="IEquatable{ProjectionActorState}" />
[DataContract]
public record ProjectionState(
    [property: DataMember] long EventStreamVersion,
    [property: DataMember] long LastEventDone)
{
}
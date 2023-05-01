// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-01-2023
// ***********************************************************************
// <copyright file="ProjectionState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using System.Runtime.Serialization;

/// <summary>
/// Class ProjectionActorState.
/// Implements the <see cref="IEquatable{ProjectionActorState}" />.
/// </summary>
/// <seealso cref="IEquatable{ProjectionActorState}" />
[DataContract]
public record ProjectionState(
    long EventStreamVersion,
    long LastEventDone)
{
}
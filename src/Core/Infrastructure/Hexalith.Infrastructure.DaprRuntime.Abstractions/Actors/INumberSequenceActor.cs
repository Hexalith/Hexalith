// <copyright file="INumberSequenceActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using Dapr.Actors;

/// <summary>
/// Interface INumberSequenceActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface INumberSequenceActor : IActor
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <returns>Task&lt;System.Nullable&lt;System.String&gt;&gt;.</returns>
    Task<long> NextAsync();
}
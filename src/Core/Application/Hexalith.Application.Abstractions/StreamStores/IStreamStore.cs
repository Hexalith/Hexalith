// <copyright file="IStreamStore.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

/// <summary>
/// Event store interface.
/// </summary>
public interface IStreamStore
{
    /// <summary>
    /// Gets an event stream. If the stream does not exist, it is created.
    /// </summary>
    /// <param name="streamId">The stream identifier.</param>
    /// <returns>The event stream.</returns>
    IPersistedStream GetStream(string streamId);
}
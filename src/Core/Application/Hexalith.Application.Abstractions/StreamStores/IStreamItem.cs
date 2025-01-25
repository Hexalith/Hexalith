// <copyright file="IStreamItem.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

/// <summary>
/// Persisted stream item interface.
/// </summary>
public interface IStreamItem
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    IDataFragment Message { get; }

    /// <summary>
    /// Gets the stream sequence number.
    /// </summary>
    long Sequence { get; }
}
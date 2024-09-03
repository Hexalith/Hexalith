// <copyright file="IStreamItem.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
    public IDataFragment Message { get; }

    /// <summary>
    /// Gets the stream sequence number.
    /// </summary>
    public long Sequence { get; }
}
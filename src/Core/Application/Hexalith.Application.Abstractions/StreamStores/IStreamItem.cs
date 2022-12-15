// <copyright file="IStreamItem.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persisted stream item iterface.
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
// <copyright file="StreamItem.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Persited stream item.
/// </summary>
[DataContract]
public class StreamItem : IStreamItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StreamItem"/> class.
    /// </summary>
    /// <param name="sequence">The sequence.</param>
    /// <param name="message">The message.</param>
    [JsonConstructor]
    public StreamItem(long sequence, IDataFragment message)
    {
        Sequence = sequence;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="StreamItem"/> class.
    /// Initializer for serializers that require a parameterless constructor.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public StreamItem() => Message = new DataFragment();

    /// <summary>
    /// Gets message data.
    /// </summary>
    public IDataFragment Message { get; }

    /// <summary>
    /// Gets or sets the stream sequence number.
    /// </summary>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public long Sequence { get; set; }
}
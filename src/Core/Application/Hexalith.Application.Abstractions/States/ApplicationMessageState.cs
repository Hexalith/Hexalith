// <copyright file="ApplicationMessageState.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.MessageMetadatas;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents the state of an application message.
/// </summary>
/// <param name="RecordMessage">The record message associated with the application message.</param>
/// <param name="ClassMessage">The class message associated with the application message.</param>
/// <param name="Metadata">The metadata associated with the message.</param>
[DataContract]
public record ApplicationMessageState(
    [property: DataMember(Order = 1)]
    PolymorphicRecordBase? RecordMessage,
    [property: DataMember(Order = 2)]
    PolymorphicClassBase? ClassMessage,
    [property: DataMember(Order = 3)]
    Metadata Metadata)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationMessageState"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    public ApplicationMessageState(object message, Metadata metadata)
        : this(message as PolymorphicRecordBase, message as PolymorphicClassBase, metadata)
    {
    }

    /// <summary>
    /// Gets the message associated with the application message state.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public object Message => RecordMessage as object ?? ClassMessage ?? throw new InvalidOperationException("The record and class message values are null.");
}
// <copyright file="MessageState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Represents the state of a message.
/// </summary>
/// <param name="Message">The message.</param>
/// <param name="Metadata">The metadata.</param>
[DataContract]
public record MessageState(
    [property: DataMember(Order = 1)]
    [property: JsonPropertyOrder(1)]
    object Message,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(2)]
    Metadata Metadata) : IIdempotent
{
    /// <inheritdoc/>
    [JsonIgnore]
    [IgnoreDataMember]
    public string IdempotencyId => Metadata.Message.Id;
}
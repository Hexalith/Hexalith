// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="MessageMetadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
Licensed under the MIT license.
    See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class MessageMetadata.
/// Implements the <see cref="Hexalith.Application.Metadatas.IMessageMetadata" />
/// </summary>
/// <seealso cref="Hexalith.Application.Metadatas.IMessageMetadata" />
[DataContract]
public class MessageMetadata : IMessageMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public MessageMetadata()
    {
        Id = Name = string.Empty;
        Version = new MessageVersion();
        Aggregate = new AggregateMetadata();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="date">The date.</param>
    /// <param name="version">The version.</param>
    /// <param name="aggregate">The aggregate.</param>
    [JsonConstructor]
    public MessageMetadata(string id, string name, DateTimeOffset date, MessageVersion version, AggregateMetadata aggregate)
    {
        Id = id;
        Name = name;
        Version = version;
        Aggregate = aggregate;
        Date = date;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="date">The date.</param>
    /// <param name="message">The message.</param>
    public MessageMetadata(string id, DateTimeOffset date, BaseMessage message)
    {
        Id = id;
        Name = message.TypeName;
        Version = new MessageVersion(message.MajorVersion, message.MinorVersion);
        Aggregate = new AggregateMetadata(message.AggregateId, message.AggregateName);
        Date = date;
    }

    /// <inheritdoc/>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public AggregateMetadata Aggregate { get; }

    /// <inheritdoc/>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public DateTimeOffset Date { get; }

    /// <inheritdoc/>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Id { get; }

    /// <inheritdoc/>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string Name { get; }

    /// <inheritdoc/>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public MessageVersion Version { get; }

    /// <inheritdoc/>
    IAggregateMetadata IMessageMetadata.Aggregate => Aggregate;

    /// <inheritdoc/>
    IMessageVersion IMessageMetadata.Version => Version;
}
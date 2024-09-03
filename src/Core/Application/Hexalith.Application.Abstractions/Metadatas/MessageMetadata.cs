// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-15-2023
// ***********************************************************************
// <copyright file="MessageMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Metadatas;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions;

/// <summary>
/// Class MessageMetadata.
/// Implements the <see cref="Hexalith.Application.Metadatas.IMessageMetadata" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Metadatas.IMessageMetadata" />
[DataContract]
public class MessageMetadata : IMessageMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
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
    /// <param name="createdDate">The date.</param>
    /// <param name="version">The version.</param>
    /// <param name="aggregate">The aggregate.</param>
    [JsonConstructor]
    public MessageMetadata(string id, string name, DateTimeOffset createdDate, MessageVersion version, AggregateMetadata aggregate)
    {
        Id = id;
        Name = name;
        Version = version;
        Aggregate = aggregate;
        CreatedDate = createdDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageMetadata" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="createdDate">The date.</param>
    /// <param name="message">The message.</param>
    /// <exception cref="System.ArgumentNullException">null argument.</exception>
    public MessageMetadata(string id, DateTimeOffset createdDate, [NotNull] BaseMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Id = id;
        Name = message.TypeName;
        Version = new MessageVersion(message.MajorVersion, message.MinorVersion);
        Aggregate = new AggregateMetadata(message.AggregateId, message.AggregateName);
        CreatedDate = createdDate;
    }

    /// <summary>
    /// Gets the aggregate.
    /// </summary>
    /// <value>The aggregate.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public AggregateMetadata Aggregate { get; }

    /// <inheritdoc/>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public DateTimeOffset CreatedDate { get; }

    /// <inheritdoc/>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Id { get; }

    /// <inheritdoc/>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string Name { get; }

    /// <summary>
    /// Gets the message version.
    /// </summary>
    /// <value>The version.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public MessageVersion Version { get; }

    /// <inheritdoc/>
    IAggregateMetadata IMessageMetadata.Aggregate => Aggregate;

    /// <inheritdoc/>
    IMessageVersion IMessageMetadata.Version => Version;
}
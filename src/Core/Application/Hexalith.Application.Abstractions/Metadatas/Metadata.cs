// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="Metadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Abstractions.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Ardalis.GuardClauses;

using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class Metadata.
/// Implements the <see cref="Hexalith.Application.Abstractions.Metadatas.IMetadata" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Abstractions.Metadatas.IMetadata" />
[DataContract]
[JsonPolymorphicBaseClass]
public class Metadata : IMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public Metadata()
    {
        Message = new MessageMetadata();
        Version = new MetadataVersion();
        Context = new ContextMetadata();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata" /> class.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="message">The message.</param>
    /// <param name="context">The context.</param>
    /// <param name="scopes">The scopes.</param>
    [JsonConstructor]
    public Metadata(
        MetadataVersion version,
        MessageMetadata message,
        ContextMetadata context,
        IEnumerable<string>? scopes)
    {
        Version = version;
        Message = message;
        Context = context;
        Scopes = scopes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="message">The message.</param>
    /// <param name="date">The date.</param>
    /// <param name="context">The context.</param>
    /// <param name="scopes">The scopes.</param>
    public Metadata(
        string id,
        IMessage message,
        DateTimeOffset date,
        ContextMetadata context,
        IEnumerable<string>? scopes)
    {
        _ = Guard.Against.Null(message);
        Version = new MetadataVersion(0, 0);
        Message = new MessageMetadata(
            id,
            message.MessageName,
            date,
            new MessageVersion(message.MajorVersion, message.MinorVersion),
            new AggregateMetadata(message.AggregateId, message.AggregateName));
        Context = context;
        Scopes = scopes;
    }

    /// <summary>
    /// Gets the message context metadata.
    /// </summary>
    /// <value>The context.</value>
    public ContextMetadata Context { get; private set; }

    /// <summary>
    /// Gets the message metadata.
    /// </summary>
    /// <value>The message.</value>
    public MessageMetadata Message { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<string>? Scopes { get; private set; }

    /// <summary>
    /// Gets the metadata version.
    /// </summary>
    /// <value>The version.</value>
    public MetadataVersion Version { get; private set; }

    /// <inheritdoc/>
    IContextMetadata IMetadata.Context => Context;

    /// <inheritdoc/>
    IMessageMetadata IMetadata.Message => Message;

    /// <inheritdoc/>
    IMetadataVersion IMetadata.Version => Version;

    /// <summary>
    /// Creates the new.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="date">The date.</param>
    /// <returns>Metadata.</returns>
    public static Metadata CreateNew(IMessage message, IMetadata metadata, DateTimeOffset date)
    {
        return new Metadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            message,
            date,
            new(metadata.Context),
            metadata.Scopes);
    }

    /// <summary>
    /// Creates the new.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="metadata">The metadata.</param>
    /// <returns>Metadata.</returns>
    public static Metadata CreateNew(IMessage message, IMetadata metadata)
    {
        return new Metadata(
            UniqueIdHelper.GenerateUniqueStringId(),
            message,
            metadata.Message.Date,
            new(metadata.Context),
            metadata.Scopes);
    }
}
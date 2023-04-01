// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="BaseMetadata.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class Metadata.
/// Implements the <see cref="Hexalith.Application.Abstractions.Metadatas.IMetadata" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Abstractions.Metadatas.IMetadata" />
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseMetadata>))]
public abstract class BaseMetadata : IMetadata, IPolymorphicSerializable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMetadata"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public BaseMetadata()
    {
        Message = new MessageMetadata();
        Context = new ContextMetadata();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMetadata"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="context">The context.</param>
    /// <param name="scopes">The scopes.</param>
    [JsonConstructor]
    public BaseMetadata(
        MessageMetadata message,
        ContextMetadata context,
        IEnumerable<string>? scopes)
    {
        Message = message;
        Context = context;
        Scopes = scopes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMetadata"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="message">The message.</param>
    /// <param name="date">The date.</param>
    /// <param name="context">The context.</param>
    /// <param name="scopes">The scopes.</param>
    public BaseMetadata(
        string id,
        IMessage message,
        DateTimeOffset date,
        ContextMetadata context,
        IEnumerable<string>? scopes)
    {
        _ = Guard.Against.Null(message);
        Message = new MessageMetadata(
            id,
            message.TypeName,
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

    /// <inheritdoc/>
    IContextMetadata IMetadata.Context => Context;

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MajorVersion => DefaultMajorVersion();

    /// <summary>
    /// Gets the message metadata.
    /// </summary>
    /// <value>The message.</value>
    public MessageMetadata Message { get; private set; }

    /// <inheritdoc/>
    IMessageMetadata IMetadata.Message => Message;

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MinorVersion => DefaultMinorVersion();

    /// <inheritdoc/>
    public IEnumerable<string>? Scopes { get; private set; }

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string TypeName => DefaultTypeName();

    /// <summary>
    /// Get the message major version.
    /// </summary>
    /// <returns>The major version.</returns>
    protected virtual int DefaultMajorVersion()
    {
        return 0;
    }

    /// <summary>
    /// Gets the message minor version.
    /// </summary>
    /// <returns>The minor version.</returns>
    protected virtual int DefaultMinorVersion()
    {
        return 0;
    }

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultTypeName()
    {
        return GetType().Name;
    }
}
// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-10-2023
// ***********************************************************************
// <copyright file="BaseMetadata.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Metadatas;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class Metadata.
/// Implements the <see cref="IMetadata" />.
/// </summary>
/// <seealso cref="IMetadata" />
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseMetadata>))]
public class BaseMetadata : IMetadata, IPolymorphicSerializable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseMetadata"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
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
        ArgumentNullException.ThrowIfNull(message);
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
    [IgnoreDataMember]
    [JsonIgnore]
    public int MajorVersion => DefaultMajorVersion();

    /// <summary>
    /// Gets the message metadata.
    /// </summary>
    /// <value>The message.</value>
    public MessageMetadata Message { get; private set; }

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public int MinorVersion => DefaultMinorVersion();

    /// <inheritdoc/>
    public IEnumerable<string>? Scopes { get; private set; }

    /// <inheritdoc/>
    public string TypeMapName => IPolymorphicSerializable.GetTypeMapName(TypeName, MajorVersion, MinorVersion);

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string TypeName => DefaultTypeName();

    /// <inheritdoc/>
    IContextMetadata IMetadata.Context => Context;

    /// <inheritdoc/>
    IMessageMetadata IMetadata.Message => Message;

    /// <summary>
    /// Get the message major version.
    /// </summary>
    /// <returns>The major version.</returns>
    protected virtual int DefaultMajorVersion() => 0;

    /// <summary>
    /// Gets the message minor version.
    /// </summary>
    /// <returns>The minor version.</returns>
    protected virtual int DefaultMinorVersion() => 0;

    /// <summary>
    /// Get the message name.
    /// </summary>
    /// <returns>The name.</returns>
    protected virtual string DefaultTypeName() => GetType().Name;
}
// <copyright file="Metadata.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Metadatas;

using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Serialization;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
[JsonPolymorphicBaseClass]
public class Metadata : IMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public Metadata()
    {
        Message = new MessageMetadata();
        Version = new MetadataVersion();
        Context = new ContextMetadata();
        ReceivedDate = DateTimeOffset.MinValue;
        EmittedDate = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    /// <param name="version"></param>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="receivedDate"></param>
    /// <param name="scopes"></param>
    [JsonConstructor]
    public Metadata(
        MetadataVersion version,
        MessageMetadata message,
        ContextMetadata context,
        DateTimeOffset emittedDate,
        DateTimeOffset receivedDate,
        IEnumerable<string>? scopes)
    {
        Version = version;
        Message = message;
        Context = context;
        EmittedDate = emittedDate;
        ReceivedDate = receivedDate;
        Scopes = scopes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Metadata"/> class.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="context"></param>
    /// <param name="scopes"></param>
    public Metadata(
        string id,
        IMessage message,
        ContextMetadata context,
        IEnumerable<string>? scopes = null)
    {
        Version = new MetadataVersion(0, 0);
        Message = new MessageMetadata(
            id,
            message.MessageName,
            new MessageVersion(message.MajorVersion, message.MinorVersion),
            new AggregateMetadata(message.AggregateId, message.AggregateName));
        CreatedDate = EmittedDate = DateTimeOffset.UtcNow;
        Context = context;
        Scopes = scopes;
    }

    /// <inheritdoc/>
    public ContextMetadata Context { get; private set; }

    /// <inheritdoc/>
    public string CorrelationId { get; }

    /// <inheritdoc/>
    public DateTimeOffset CreatedDate { get; }

    public DateTimeOffset EmittedDate { get; }

    /// <inheritdoc/>
    public MessageMetadata Message { get; private set; }

    /// <inheritdoc/>
    public DateTimeOffset ReceivedDate { get; private set; }

    /// <inheritdoc/>
    public IEnumerable<string>? Scopes { get; private set; }

    /// <inheritdoc/>
    public MetadataVersion Version { get; private set; }

    /// <inheritdoc/>
    IContextMetadata IMetadata.Context => Context;

    /// <inheritdoc/>
    IMessageMetadata IMetadata.Message => Message;

    /// <inheritdoc/>
    IMetadataVersion IMetadata.Version => Version;
}

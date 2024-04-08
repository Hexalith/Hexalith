// <copyright file="Dynamics365BusinessEventBase.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.BusinessEvents;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Serialization;
using Hexalith.Infrastructure.Serialization.Serialization;

/// <summary>
/// The dynamics365 business event metadata.
/// </summary>
[DataContract]
[JsonConverter(typeof(BusinessEventJsonConverter))]
public abstract class Dynamics365BusinessEventBase : IMetadata, IEvent
{
    private string? _businessEventId;
    private int _majorVersion;
    private int _minorVersion;

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public virtual string AggregateId => string.Empty;

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public virtual string AggregateName => string.Empty;

    /// <summary>
    /// Gets or sets the business event id.
    /// </summary>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string? BusinessEventId
    {
        get => string.IsNullOrEmpty(_businessEventId) ? DefaultTypeName() : _businessEventId;
        set => _businessEventId = value;
    }

    /// <summary>
    /// Gets or sets the business event legal entity.
    /// </summary>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string? BusinessEventLegalEntity { get; set; }

    /// <summary>
    /// Gets the user id.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public IContextMetadata Context => new ContextMetadata(
        EventId ?? string.Empty,
        InitiatingUserAzureActiveDirectoryObjectId ?? string.Empty,
        ReceivedDateTime ?? DateTimeOffset.MinValue,
        sequenceNumber: null,
        sessionId: null);

    /// <summary>
    /// Gets or sets the control number.
    /// </summary>
    [DataMember(Order = 9)]
    [JsonPropertyOrder(9)]
    public long ControlNumber { get; set; }

    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event time.
    /// </summary>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    [JsonConverter(typeof(UnixEpochDateTimeConverter))]
    public DateTime? EventTime { get; set; }

    /// <summary>
    /// Gets or sets the event time iso8601.
    /// </summary>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    [JsonConverter(typeof(IsoUtcDateTimeOffsetConverter))]
    public DateTimeOffset? EventTimeIso8601 { get; set; }

    /// <summary>
    /// Gets or sets the initiating user a a d object id.
    /// </summary>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    [JsonPropertyName("InitiatingUserAADObjectId")]
    public string? InitiatingUserAzureActiveDirectoryObjectId { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance is private to aggregate.
    /// </summary>
    /// <value><c>true</c> if this instance is private to aggregate; otherwise, <c>false</c>.</value>
    public bool IsPrivateToAggregate => false;

    /// <summary>
    /// Gets or sets the major version.
    /// </summary>
    [DataMember(Order = 7)]
    [JsonPropertyOrder(7)]
    public int MajorVersion { get => _majorVersion <= 0 ? DefaultMajorVersion() : _majorVersion; set => _majorVersion = value; }

    /// <summary>
    /// Gets the message id.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public IMessageMetadata Message => new MessageMetadata(
        EventId ?? string.Empty,
        BusinessEventId ?? string.Empty,
        ReceivedDateTime ?? DateTimeOffset.MinValue,
        new MessageVersion(
            MajorVersion,
            MinorVersion),
        new AggregateMetadata(string.Empty, string.Empty));

    /// <summary>
    /// Gets or sets the minor version.
    /// </summary>
    [DataMember(Order = 8)]
    [JsonPropertyOrder(8)]
    public int MinorVersion { get => _minorVersion <= 0 ? DefaultMinorVersion() : _minorVersion; set => _minorVersion = value; }

    /// <summary>
    /// Gets or sets the origin identifier.
    /// </summary>
    /// <value>The origin identifier.</value>
    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public string? OriginId { get; set; }

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string? PartitionId { get; set; }

    /// <summary>
    /// Gets the received date time.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public DateTimeOffset? ReceivedDateTime => EventTime ?? EventTimeIso8601;

    /// <summary>
    /// Gets the correlation id.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public IEnumerable<string>? Scopes => [];

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string TypeName => string.IsNullOrWhiteSpace(BusinessEventId) ? DefaultTypeName() : BusinessEventId;

    /// <summary>
    /// Gets the user id.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public IMetadataVersion Version => new MetadataVersion(MajorVersion, MinorVersion);

    /// <summary>
    /// Gets the business command.
    /// </summary>
    /// <returns>The command.</returns>
    public virtual IEnumerable<BaseCommand> ToCommands() => [];

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
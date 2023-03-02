// <copyright file="Dynamics365BusinessEventBase.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Serialization;
using Hexalith.Infrastructure.Serialization.Serialization;

/// <summary>
/// The dynamics365 business event metadata.
/// </summary>
[JsonPolymorphicBaseClass]
[DataContract]
public class Dynamics365BusinessEventBase : IMetadata, IEvent
{
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
    public string? BusinessEventId { get; set; }

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
    /// Gets or sets the major version.
    /// </summary>
    [DataMember(Order = 7)]
    [JsonPropertyOrder(7)]
    public int MajorVersion { get; set; }

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

    /// <inheritdoc/>
    [IgnoreDataMember]
    [JsonIgnore]
    public string MessageName => BusinessEventId ?? string.Empty;

    /// <summary>
    /// Gets or sets the minor version.
    /// </summary>
    [DataMember(Order = 8)]
    [JsonPropertyOrder(8)]
    public int MinorVersion { get; set; }

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
    public IEnumerable<string>? Scopes => Array.Empty<string>();

    /// <summary>
    /// Gets the user id.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public IMetadataVersion Version => new MetadataVersion(MajorVersion, MinorVersion);

    /// <summary>
    /// Adds the polymorphic type property set with BusinessEventId value and deserialize object.
    /// Standard .NET polymorphic deserialization needs the type to be the first property in the JSON. We cannot guarantee that the BusinessEventId will be first.
    /// </summary>
    /// <param name="json">The json.</param>
    /// <returns>Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents.Dynamics365BusinessEventBase.</returns>
    /// <exception cref="InvalidOperationException">Unable to deserialize business event. The BusinessEventId property is missing or empty.</exception>
    public static Dynamics365BusinessEventBase AddTypeAndDeserialize(JsonElement json)
    {
        _ = Guard.Against.Null(json);
        string? name;
        try
        {
            name = json.GetProperty(nameof(Dynamics365BusinessEventBase.BusinessEventId)).GetString();
        }
        catch (Exception e)
        {
            throw new SerializationException("Unable to deserialize business event property BusinessEventId.", e);
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new SerializationException("The BusinessEventId property is empty.");
        }

        // Add polymorphic type property value
        string jsonText = "{\"$type\":\"" + name + "\"," + json.GetRawText().Trim()[1..];

        return Deserialize(jsonText);
    }

    /// <summary>Deserializes the specified json.</summary>
    /// <param name="json">The json.</param>
    /// <returns>Dynamics365BusinessEventBase.</returns>
    /// <exception cref="System.InvalidOperationException">Unable to deserialize business event.</exception>
    public static Dynamics365BusinessEventBase Deserialize(string json)
    {
        _ = Guard.Against.NullOrWhiteSpace(json);
        Dynamics365BusinessEventBase? result = JsonSerializer
            .Deserialize<Dynamics365BusinessEventBase>(
                json,
                new JsonSerializerOptions
                {
                    TypeInfoResolver = new PolymorphicTypeResolver(),
                });
        return result ?? throw new InvalidOperationException("Unable to deserialize business event.");
    }

    /// <summary>
    /// Serializes the specified business event.
    /// </summary>
    /// <param name="businessEvent">The business event.</param>
    /// <returns>System.String.</returns>
    public static string Serialize(Dynamics365BusinessEventBase businessEvent)
    {
        _ = Guard.Against.Null(businessEvent);
        return JsonSerializer.Serialize(businessEvent);
    }

    /// <summary>
    /// Gets the business command.
    /// </summary>
    /// <returns>The command.</returns>
    public virtual BaseCommand ToCommand()
    {
        throw new NotSupportedException();
    }
}
// <copyright file="Dynamics365BusinessEventBase.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Helpers;
using Hexalith.Infrastructure.Serialization;

using System;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// The dynamics365 business event metadata.
/// </summary>
[JsonPolymorphicBaseClass]
public class Dynamics365BusinessEventBase : IMetadata, IEvent
{
    /// <inheritdoc/>
    public virtual string AggregateId { get => string.Empty; }

    /// <inheritdoc/>
    public virtual string AggregateName { get => string.Empty; }

    /// <summary>
    /// Gets or sets the business event id.
    /// </summary>
    [DataMember]
    public string? BusinessEventId { get; set; }

    /// <summary>
    /// Gets or sets the business event legal entity.
    /// </summary>
    [DataMember]
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
    /// Gets or sets the context record subject.
    /// </summary>
    [DataMember]
    public string? ContextRecordSubject { get; set; }

    /// <summary>
    /// Gets or sets the control number.
    /// </summary>
    [DataMember]
    public long ControlNumber { get; set; }

    /// <summary>
    /// Gets or sets the event id.
    /// </summary>
    [DataMember]
    public string? EventId { get; set; }

    /// <summary>
    /// Gets or sets the event time.
    /// </summary>
    [DataMember]
    public string? EventTime { get; set; }

    /// <summary>
    /// Gets or sets the event time iso8601.
    /// </summary>
    [DataMember]
    public DateTimeOffset? EventTimeIso8601 { get; set; }

    /// <summary>
    /// Gets or sets the initiating user a a d object id.
    /// </summary>
    [DataMember]
    [JsonPropertyName("InitiatingUserAADObjectId")]
    public string? InitiatingUserAzureActiveDirectoryObjectId { get; set; }

    /// <summary>
    /// Gets or sets the major version.
    /// </summary>
    [DataMember]
    public int MajorVersion { get; set; }

    /// <summary>
    /// Gets the message id.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public IMessageMetadata Message => new MessageMetadata(
        EventId ?? string.Empty,
        BusinessEventId ?? string.Empty,
        EventTimeIso8601 ?? DateTimeOffset.MinValue,
        new MessageVersion(
            MajorVersion,
            MinorVersion),
        new AggregateMetadata(string.Empty, string.Empty));

    /// <inheritdoc/>
    public string MessageName => BusinessEventId ?? string.Empty;

    /// <summary>
    /// Gets or sets the minor version.
    /// </summary>
    [DataMember]
    public int MinorVersion { get; set; }

    /// <summary>
    /// Gets or sets the received date time.
    /// </summary>
    [IgnoreDataMember]
    [JsonIgnore]
    public DateTimeOffset? ReceivedDateTime => EventTimeIso8601 ?? Dynamics365BusinessEventHelper.ParseDynamics365EventTimeOffset(EventTime);

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
    public virtual BaseCommand ToCommand() => throw new NotSupportedException();
}
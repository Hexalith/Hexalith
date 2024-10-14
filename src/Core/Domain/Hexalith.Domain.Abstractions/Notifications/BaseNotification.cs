// <copyright file="BaseNotification.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Notifications;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Represents a base notification.
/// </summary>
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseNotification>))]
[Obsolete]
public class BaseNotification : BaseMessage, INotification
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNotification"/> class.
    /// </summary>
    /// <param name="sourceAggregateName">The name of the source aggregate.</param>
    /// <param name="sourceAggregateId">The source aggregate identifier.</param>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The message of the notification.</param>
    /// <param name="severity">The severity of the notification.</param>
    /// <param name="technicalDescription">The technical description of the notification.</param>
    [JsonConstructor]
    public BaseNotification(
        string sourceAggregateName,
        string sourceAggregateId,
        string title,
        string message,
        NotificationSeverity severity,
        string? technicalDescription)
    {
        Title = title;
        Message = message;
        Severity = severity;
        TechnicalDescription = technicalDescription;
        SourceAggregateId = sourceAggregateId;
        SourceAggregateName = sourceAggregateName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseNotification"/> class.
    /// </summary>
    [Obsolete("For serialization only", true)]
    public BaseNotification() => SourceAggregateId = SourceAggregateName = Title = Message = string.Empty;

    /// <summary>
    /// Gets the undefined aggregate identifier.
    /// </summary>
    public static string UndefinedAggregateId => "Undefined";

    /// <summary>
    /// Gets the undefined aggregate name.
    /// </summary>
    public static string UndefinedAggregateName => "Undefined";

    /// <summary>
    /// Gets or sets the message of the notification.
    /// </summary>
    [JsonPropertyOrder(6)]
    [DataMember(Order = 6)]
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the severity of the notification.
    /// </summary>
    [JsonPropertyOrder(1)]
    [DataMember(Order = 1)]
    public NotificationSeverity Severity { get; set; }

    /// <summary>
    /// Gets or sets the source aggregate identifier.
    /// </summary>
    [JsonPropertyOrder(4)]
    [DataMember(Order = 4)]
    public string? SourceAggregateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the source aggregate.
    /// </summary>
    [JsonPropertyOrder(3)]
    [DataMember(Order = 3)]
    public string? SourceAggregateName { get; set; }

    /// <summary>
    /// Gets or sets the technical description of the notification.
    /// </summary>
    [JsonPropertyOrder(7)]
    [DataMember(Order = 7)]
    public string? TechnicalDescription { get; set; }

    /// <summary>
    /// Gets or sets the title of the notification.
    /// </summary>
    [JsonPropertyOrder(5)]
    [DataMember(Order = 5)]
    public string Title { get; set; }

    /// <summary>
    /// Creates a notification message for the specified aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="severity">The severity.</param>
    /// <param name="technicalDescription">The technical description.</param>
    /// <returns>BaseNotification.</returns>
    public static BaseNotification Create(
            [NotNull] IDomainAggregate aggregate,
            string title,
            string message,
            NotificationSeverity severity,
            string? technicalDescription)
    {
        return new BaseNotification(
            (aggregate ?? throw new ArgumentNullException(nameof(aggregate))).AggregateName,
            aggregate.AggregateId,
            title,
            message,
            severity,
            technicalDescription);
    }

    /// <summary>
    /// Creates an error notification message for the specified aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="technicalDescription">The technical description.</param>
    /// <returns>BaseNotification.</returns>
    public static BaseNotification CreateError(
            [NotNull] IDomainAggregate aggregate,
            string title,
            string message,
            string? technicalDescription)
    {
        return Create(
            aggregate,
            title,
            message,
            NotificationSeverity.Error,
            technicalDescription);
    }

    /// <summary>
    /// Creates an information notification message for the specified aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="technicalDescription">The technical description.</param>
    /// <returns>BaseNotification.</returns>
    public static BaseNotification CreateInformation(
            [NotNull] IDomainAggregate aggregate,
            string title,
            string message,
            string? technicalDescription)
    {
        return Create(
            aggregate,
            title,
            message,
            NotificationSeverity.Information,
            technicalDescription);
    }

    /// <summary>
    /// Creates an warning notification message for the specified aggregate.
    /// </summary>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="title">The title.</param>
    /// <param name="message">The message.</param>
    /// <param name="technicalDescription">The technical description.</param>
    /// <returns>BaseNotification.</returns>
    public static BaseNotification CreateWarning(
            [NotNull] IDomainAggregate aggregate,
            string title,
            string message,
            string? technicalDescription)
    {
        return Create(
            aggregate,
            title,
            message,
            NotificationSeverity.Warning,
            technicalDescription);
    }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => SourceAggregateId ?? UndefinedAggregateId;

    /// <inheritdoc/>
    protected override string DefaultAggregateName()
        => string.IsNullOrWhiteSpace(SourceAggregateName) ? UndefinedAggregateName : SourceAggregateName;
}
// <copyright file="Survey.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Entities;
using Hexalith.Domain.Events;
using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.Domains.ValueObjects;

/// <summary>
/// Represents a survey aggregate.
/// </summary>
/// <param name="Id">The unique identifier of the survey.</param>
/// <param name="Name">The name of the survey.</param>
/// <param name="Categories">The categories associated with the survey.</param>
/// <param name="Users">The users associated with the survey.</param>
/// <param name="Period">The period of the survey.</param>
/// <param name="StartDate">The start date of the survey.</param>
/// <param name="EndDate">The end date of the survey.</param>
[DataContract]
public record Survey(
    string Id,
    [property: DataMember(Order = 5)] string Name,
    [property: DataMember(Order = 6)] IEnumerable<SurveyCategory> Categories,
    [property: DataMember(Order = 7)] IEnumerable<SurveyUser> Users,
    [property: DataMember(Order = 8)] SurveyPeriod Period,
    [property: DataMember(Order = 9)] DateTimeOffset StartDate,
    [property: DataMember(Order = 10)] DateTimeOffset EndDate) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Survey" /> class.
    /// </summary>
    /// <param name="registered">The survey registered event.</param>
    /// <exception cref="ArgumentNullException">Thrown when the registered event is null.</exception>
    public Survey(SurveyRegistered registered)
        : this(
              (registered ?? throw new ArgumentNullException(nameof(registered))).Id,
              registered.Name,
              registered.Categories,
              registered.Users,
              registered.Period,
              registered.StartDate,
              registered.EndDate)
    {
    }

    /// <inheritdoc/>
    public string DomainId => SurveyDomainHelper.BuildSurveyDomainId(Id);

    /// <inheritdoc/>
    public string DomainName => SurveyDomainHelper.SurveyDomainName;

    /// <inheritdoc/>
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is SurveyRegistered added)
        {
            return !IsInitialized()
                ? ApplyEvent(added)
                : new ApplyResult(
                this,
                [new SurveyEventCancelled(added, $"Aggregate {Id}/{Name} already initialized")],
                true);
        }

        if (domainEvent is SurveyEvent contactEvent)
        {
            if (contactEvent.DomainId != DomainId)
            {
                return new ApplyResult(this, [new SurveyEventCancelled(contactEvent, $"Invalid aggregate identifier for {Id}/{Name} : {contactEvent.DomainId}")], true);
            }
        }
        else
        {
            return new ApplyResult(
                this,
                [new InvalidEventApplied(
                    DomainName,
                    DomainId,
                    domainEvent.GetType().FullName ?? "Unknown",
                    JsonSerializer.Serialize(domainEvent),
                    $"Unexpected event applied.")],
                true);
        }

        return contactEvent switch
        {
            SurveyInformationChanged e => ApplyEvent(e),
            _ => new ApplyResult(
                this,
                [new SurveyEventCancelled(contactEvent, "Event not implemented")],
                true),
        };
    }

    /// <summary>
    /// Applies the survey registered event.
    /// </summary>
    /// <param name="added">The survey registered event.</param>
    /// <returns>The result of applying the event.</returns>
    private ApplyResult ApplyEvent(SurveyRegistered added)
        => new(new Survey(added), [added], false);

    /// <summary>
    /// Applies the survey information changed event.
    /// </summary>
    /// <param name="changed">The survey information changed event.</param>
    /// <returns>The result of applying the event.</returns>
    private ApplyResult ApplyEvent(SurveyInformationChanged changed)
        => new(
            this with
            {
                Name = changed.Name,
            },
            [changed],
            false);
}
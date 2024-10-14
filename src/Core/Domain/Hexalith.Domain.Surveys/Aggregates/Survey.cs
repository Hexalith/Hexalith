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
using Hexalith.Domain.ValueObjects;

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
    /// <param name="registered">The customer.</param>
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
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    public string AggregateId => SurveyDomainHelper.BuildSurveyAggregateId(Id);

    /// <inheritdoc/>
    public string AggregateName => SurveyDomainHelper.SurveyAggregateName;

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
            if (contactEvent.AggregateId != AggregateId)
            {
                return new ApplyResult(this, [new SurveyEventCancelled(contactEvent, $"Invalid aggregate identifier for {Id}/{Name} : {contactEvent.AggregateId}")], true);
            }
        }
        else
        {
            return new ApplyResult(
                this,
                [new InvalidEventApplied(
                    AggregateName,
                    AggregateId,
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

    private ApplyResult ApplyEvent(SurveyRegistered added)
        => new(new Survey(added), [added], false);

    private ApplyResult ApplyEvent(SurveyInformationChanged changed)
        => new(
            this with
            {
                Name = changed.Name,
            },
            [changed],
            false);
}
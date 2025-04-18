// <copyright file="SurveyCampaign.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json;

using Hexalith.Domain.Events;
using Hexalith.Domains;
using Hexalith.Domains.Results;

/// <summary>
/// Represents a survey campaign in the domain.
/// </summary>
/// <param name="Id">The unique identifier of the survey campaign.</param>
/// <param name="Name">The name of the survey campaign.</param>
/// <param name="Date">The date of the survey campaign.</param>
[DataContract]
public record SurveyCampaign(
    string Id,
    string Name,
    DateTimeOffset Date) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyCampaign" /> class.
    /// </summary>
    /// <param name="registered">The registered survey campaign event.</param>
    /// <exception cref="ArgumentNullException">Thrown when registered is null.</exception>
    public SurveyCampaign(SurveyCampaignRegistered registered)
        : this(
              (registered ?? throw new ArgumentNullException(nameof(registered))).Id,
              registered.Name,
              registered.Date)
    {
    }

    /// <inheritdoc/>
    public string AggregateId => SurveyDomainHelper.BuildSurveyCampaignAggregateId(Id);

    /// <inheritdoc/>
    public string AggregateName => SurveyDomainHelper.SurveyCampaignAggregateName;

    /// <inheritdoc/>
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is SurveyCampaignRegistered registered)
        {
            return !IsInitialized()
                ? ApplyEvent(registered)
                : new ApplyResult(
                this,
                [new SurveyCampaignEventCancelled(registered, $"Aggregate {Id}/{Name} already initialized")],
                true);
        }

        if (domainEvent is SurveyCampaignEvent contactEvent)
        {
            if (contactEvent.AggregateId != AggregateId)
            {
                return new ApplyResult(this, [new SurveyCampaignEventCancelled(contactEvent, $"Invalid aggregate identifier for {Id}/{Name} : {contactEvent.AggregateId}")], true);
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
            SurveyCampaignInformationChanged e => ApplyEvent(e),
            _ => new ApplyResult(
                this,
                [new SurveyCampaignEventCancelled(contactEvent, "Event not implemented")],
                true),
        };
    }

    /// <summary>
    /// Applies the SurveyCampaignRegistered event to create a new SurveyCampaign.
    /// </summary>
    /// <param name="added">The SurveyCampaignRegistered event to apply.</param>
    /// <returns>An ApplyResult containing the new SurveyCampaign instance and the applied event.</returns>
    private ApplyResult ApplyEvent(SurveyCampaignRegistered added)
        => new(new SurveyCampaign(added), [added], false);

    /// <summary>
    /// Applies the SurveyCampaignInformationChanged event to update the SurveyCampaign.
    /// </summary>
    /// <param name="changed">The SurveyCampaignInformationChanged event to apply.</param>
    /// <returns>An ApplyResult containing the updated SurveyCampaign instance and the applied event.</returns>
    private ApplyResult ApplyEvent(SurveyCampaignInformationChanged changed)
        => new(
            this with
            {
                Name = changed.Name,
            },
            [changed],
            false);
}
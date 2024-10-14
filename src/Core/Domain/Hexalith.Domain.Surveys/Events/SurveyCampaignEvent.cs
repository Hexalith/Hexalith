// <copyright file="SurveyCampaignEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an abstract base class for survey campaign events.
/// </summary>
/// <param name="Id">The unique identifier of the survey campaign.</param>
/// <remarks>
/// This class serves as a base for all survey campaign-related events in the domain.
/// It uses polymorphic serialization to support different event types derived from this base class.
/// </remarks>
[PolymorphicSerialization]
public abstract record SurveyCampaignEvent(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier for the survey campaign.
    /// </summary>
    /// <remarks>
    /// This property uses the <see cref="SurveyDomainHelper.BuildSurveyCampaignAggregateId"/> method
    /// to construct the aggregate identifier based on the survey campaign's Id.
    /// </remarks>
    public string AggregateId
        => SurveyDomainHelper.BuildSurveyCampaignAggregateId(Id);

    /// <summary>
    /// Gets the name of the aggregate associated with this event.
    /// </summary>
    /// <remarks>
    /// This property returns the name of the SurveyCampaign aggregate using the
    /// <see cref="SurveyDomainHelper.SurveyCampaignAggregateName"/> property.
    /// </remarks>
    public string AggregateName
        => SurveyDomainHelper.SurveyCampaignAggregateName;
}

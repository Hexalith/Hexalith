// <copyright file="SurveyCampaignRegistered.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when a survey campaign is registered.
/// </summary>
/// <param name="Id">The unique identifier of the survey campaign.</param>
/// <param name="Name">The name of the survey campaign.</param>
/// <param name="Date">The date and time when the survey campaign was registered.</param>
[PolymorphicSerialization]
public record SurveyCampaignRegistered(
        string Id,
        string Name,
        DateTimeOffset Date) : SurveyCampaignEvent(Id);

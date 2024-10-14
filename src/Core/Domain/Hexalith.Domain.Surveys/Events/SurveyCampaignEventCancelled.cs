﻿// <copyright file="SurveyCampaignEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event indicating that a survey campaign event has been cancelled.
/// </summary>
/// <param name="Event">The original survey campaign event that was cancelled.</param>
/// <param name="Reason">The reason for cancelling the survey campaign event.</param>
[PolymorphicSerialization]
public partial record SurveyCampaignEventCancelled(
    SurveyCampaignEvent Event,
    string Reason) : SurveyCampaignEvent(Event.Id);

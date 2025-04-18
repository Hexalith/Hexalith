// <copyright file="SurveyCampaignInformationChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event that occurs when the information of a survey campaign has changed.
/// </summary>
/// <param name="Id">The unique identifier of the survey campaign.</param>
/// <param name="Name">The updated name of the survey campaign.</param>
/// <param name="Date">The date and time when the information was changed.</param>
[PolymorphicSerialization]
public partial record SurveyCampaignInformationChanged(
        string Id,
        string Name,
        DateTimeOffset Date) : SurveyCampaignEvent(Id);
// <copyright file="SurveyInformationChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when the information of a survey has been changed.
/// </summary>
/// <param name="Id">The unique identifier of the survey.</param>
/// <param name="Name">The updated name of the survey.</param>
/// <param name="Date">The date and time when the survey information was changed.</param>
[PolymorphicSerialization]
public record SurveyInformationChanged(
        string Id,
        string Name,
        DateTimeOffset Date) : SurveyEvent(Id)
{
}
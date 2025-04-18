// <copyright file="SurveyRegistered.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.Domain.Entities;
using Hexalith.Domain.ValueObjects;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an event that occurs when a new survey is registered in the system.
/// </summary>
/// <param name="Id">The unique identifier for the survey.</param>
/// <param name="Name">The name of the survey.</param>
/// <param name="Categories">The categories associated with the survey.</param>
/// <param name="Users">The users associated with the survey.</param>
/// <param name="Period">The period of the survey.</param>
/// <param name="StartDate">The start date of the survey.</param>
/// <param name="EndDate">The end date of the survey.</param>
[PolymorphicSerialization]
public partial record SurveyRegistered(
        string Id,
        string Name,
        IEnumerable<SurveyCategory> Categories,
        IEnumerable<SurveyUser> Users,
        SurveyPeriod Period,
        DateTimeOffset StartDate,
        DateTimeOffset EndDate) : SurveyEvent(Id);
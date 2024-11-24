// <copyright file="SurveyEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents an event that occurs when a survey is cancelled.
/// </summary>
/// <param name="Event">The survey event that is being cancelled.</param>
/// <param name="Reason">The reason for the survey cancellation.</param>
/// <remarks>
/// This event is used to indicate that a survey has been cancelled and provides the reason for the cancellation.
/// </remarks>
[PolymorphicSerialization]
public partial record SurveyEventCancelled(SurveyEvent Event, string Reason) : SurveyEvent(Event.Id);
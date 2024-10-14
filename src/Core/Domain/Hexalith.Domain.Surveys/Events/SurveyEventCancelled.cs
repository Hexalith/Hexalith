// <copyright file="SurveyEventCancelled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Represents a cancelled conversation thread event in the domain.
/// </summary>
/// <remarks>
/// This record is used to indicate that a previously scheduled or initiated conversation thread event
/// has been cancelled. It contains information about the original event and the reason for cancellation.
/// </remarks>
[PolymorphicSerialization]
public partial record SurveyEventCancelled(
    /// <summary>
    /// The original conversation thread event that was cancelled.
    /// </summary>
    SurveyEvent Event,

    /// <summary>
    /// The reason for cancelling the event.
    /// </summary>
    string Reason) : SurveyEvent(Event.Id);
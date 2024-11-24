// <copyright file="EventBusSettingsValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Buses;

using FluentValidation;

/// <summary>
/// Class EventBusSettingsValidator.
/// Implements the <see cref="AbstractValidator{EventBusSettings}" />.
/// </summary>
/// <seealso cref="AbstractValidator{EventBusSettings}" />
public class EventBusSettingsValidator : AbstractValidator<EventBusSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventBusSettingsValidator" /> class.
    /// </summary>
    public EventBusSettingsValidator() => _ = RuleFor(c => c.Name).NotEmpty();
}
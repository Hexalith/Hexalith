// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : jpiquot
// Created          : 02-18-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="EventBusSettingsValidator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
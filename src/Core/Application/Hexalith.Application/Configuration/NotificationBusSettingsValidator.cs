// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : jpiquot
// Created          : 02-18-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="CommandBusSettingsValidator - Copy.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Configuration;

using FluentValidation;

/// <summary>
/// Class NotificationBusSettingsValidator.
/// Implements the <see cref="FluentValidation.AbstractValidator{Hexalith.Application.Configuration.NotificationBusSettings}" />.
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator{Hexalith.Application.Configuration.NotificationBusSettings}" />
public class NotificationBusSettingsValidator : AbstractValidator<NotificationBusSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationBusSettingsValidator" /> class.
    /// </summary>
    public NotificationBusSettingsValidator()
    {
        _ = RuleFor(c => c.Name).NotEmpty();
    }
}

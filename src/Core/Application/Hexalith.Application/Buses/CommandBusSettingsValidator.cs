// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : jpiquot
// Created          : 02-18-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="CommandBusSettingsValidator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Buses;

using FluentValidation;

/// <summary>
/// Class CommandBusSettingsValidator.
/// Implements the <see cref="AbstractValidator{CommandBusSettings}" />.
/// </summary>
/// <seealso cref="AbstractValidator{CommandBusSettings}" />
public class CommandBusSettingsValidator : AbstractValidator<CommandBusSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBusSettingsValidator" /> class.
    /// </summary>
    public CommandBusSettingsValidator() => _ = RuleFor(c => c.Name).NotEmpty();
}
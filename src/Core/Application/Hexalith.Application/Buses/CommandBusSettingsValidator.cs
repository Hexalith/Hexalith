// <copyright file="CommandBusSettingsValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

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
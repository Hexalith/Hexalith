// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : jpiquot
// Created          : 02-18-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="CommandBusSettingsValidator.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Configuration;

using FluentValidation;

/// <summary>
/// Class CommandBusSettingsValidator.
/// Implements the <see cref="FluentValidation.AbstractValidator{Hexalith.Application.Configuration.CommandBusSettings}" />.
/// </summary>
/// <seealso cref="FluentValidation.AbstractValidator{Hexalith.Application.Configuration.CommandBusSettings}" />
public class CommandBusSettingsValidator : AbstractValidator<CommandBusSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandBusSettingsValidator" /> class.
    /// </summary>
    public CommandBusSettingsValidator()
    {
        _ = RuleFor(c => c.Name).NotEmpty();
    }
}

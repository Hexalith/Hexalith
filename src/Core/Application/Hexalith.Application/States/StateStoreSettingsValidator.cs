// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : jpiquot
// Created          : 02-18-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="StateStoreSettingsValidator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using FluentValidation;

/// <summary>
/// Class StateStoreSettingsValidator.
/// Implements the <see cref="AbstractValidator{StateStoreSettings}" />.
/// </summary>
/// <seealso cref="AbstractValidator{StateStoreSettings}" />
public class StateStoreSettingsValidator : AbstractValidator<StateStoreSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StateStoreSettingsValidator" /> class.
    /// </summary>
    public StateStoreSettingsValidator() => _ = RuleFor(c => c.Name).NotEmpty();
}
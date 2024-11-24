// <copyright file="StateStoreSettingsValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

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
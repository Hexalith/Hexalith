// <copyright file="RequestBusSettingsValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Buses;

using FluentValidation;

/// <summary>
/// Class RequestBusSettingsValidator.
/// Implements the <see cref="AbstractValidator{RequestBusSettings}" />.
/// </summary>
/// <seealso cref="AbstractValidator{RequestBusSettings}" />
public class RequestBusSettingsValidator : AbstractValidator<RequestBusSettings>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestBusSettingsValidator" /> class.
    /// </summary>
    public RequestBusSettingsValidator() => _ = RuleFor(c => c.Name).NotEmpty();
}
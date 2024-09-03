// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : jpiquot
// Created          : 02-18-2023
//
// Last Modified By : jpiquot
// Last Modified On : 02-18-2023
// ***********************************************************************
// <copyright file="RequestBusSettingsValidator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
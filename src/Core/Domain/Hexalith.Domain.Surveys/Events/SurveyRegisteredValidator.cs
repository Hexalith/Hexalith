// <copyright file="SurveyRegisteredValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using FluentValidation;

/// <summary>
/// Class SurveyRegisteredValidator.
/// </summary>
public class SurveyRegisteredValidator : AbstractValidator<SurveyRegistered>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyRegisteredValidator"/> class.
    /// </summary>
    public SurveyRegisteredValidator() => _ = RuleFor(x => x.Id).NotEmpty();
}
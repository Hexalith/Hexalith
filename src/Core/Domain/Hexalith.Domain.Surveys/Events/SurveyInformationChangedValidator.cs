// <copyright file="SurveyInformationChangedValidator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using FluentValidation;

/// <summary>
/// Class SurveyInformationChangedValidator.
/// </summary>
[Obsolete]
public class SurveyInformationChangedValidator : AbstractValidator<SurveyInformationChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyInformationChangedValidator"/> class.
    /// </summary>
    public SurveyInformationChangedValidator()
    {
        _ = RuleFor(x => x.Id).NotEmpty();
    }
}
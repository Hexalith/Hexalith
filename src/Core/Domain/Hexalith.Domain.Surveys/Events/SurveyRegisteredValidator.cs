// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 11-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-01-2023
// ***********************************************************************
// <copyright file="SurveyRegisteredValidator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
    public SurveyRegisteredValidator()
    {
        _ = RuleFor(x => x.PartitionId).NotEmpty();
        _ = RuleFor(x => x.CompanyId).NotEmpty();
        _ = RuleFor(x => x.Id).NotEmpty();
    }
}
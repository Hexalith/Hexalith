// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 11-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-01-2023
// ***********************************************************************
// <copyright file="SurveyInformationChangedValidator.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.Events;

using FluentValidation;

/// <summary>
/// Class SurveyInformationChangedValidator.
/// </summary>
public class SurveyInformationChangedValidator : AbstractValidator<SurveyInformationChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurveyInformationChangedValidator"/> class.
    /// </summary>
    public SurveyInformationChangedValidator()
    {
        _ = RuleFor(x => x.PartitionId).NotEmpty();
        _ = RuleFor(x => x.CompanyId).NotEmpty();
        _ = RuleFor(x => x.Id).NotEmpty();
    }
}
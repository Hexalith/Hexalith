// ***********************************************************************
// Assembly         : Hexalith.Domain.Surveys
// Author           : Jérôme Piquot
// Created          : 11-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-29-2023
// ***********************************************************************
// <copyright file="SurveyPeriodType.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.ValueObjects;

/// <summary>
/// Represents the period of a survey.
/// </summary>
public enum SurveyPeriodType
{
    /// <summary>
    /// The once.
    /// </summary>
    Once,

    /// <summary>
    /// The daily.
    /// </summary>
    Daily,

    /// <summary>
    /// The weekly.
    /// </summary>
    Weekly,

    /// <summary>
    /// The monthly.
    /// </summary>
    Monthly,

    /// <summary>
    /// The quarterly.
    /// </summary>
    Quarterly,

    /// <summary>
    /// The yearly.
    /// </summary>
    Yearly,

    /// <summary>
    /// The custom.
    /// </summary>
    Custom,
}
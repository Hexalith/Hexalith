// <copyright file="SurveyPeriodType.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domains.ValueObjects;

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
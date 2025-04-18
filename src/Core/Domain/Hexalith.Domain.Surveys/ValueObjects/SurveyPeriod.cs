// <copyright file="SurveyPeriod.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domains.ValueObjects;

using System.Diagnostics;
using System.Runtime.Serialization;

/// <summary>
/// Represents the sampling period of a survey.
/// </summary>
[DataContract]
[DebuggerDisplay("{Type} {CustomPeriod}")]
public class SurveyPeriod(SurveyPeriodType type, string? customCronPeriod)
{
    /// <summary>
    /// Gets the daily.
    /// </summary>
    /// <value>The daily.</value>
    public static SurveyPeriod Daily => new(SurveyPeriodType.Daily, null);

    /// <summary>
    /// Gets the monthly.
    /// </summary>
    /// <value>The monthly.</value>
    public static SurveyPeriod Monthly => new(SurveyPeriodType.Monthly, null);

    /// <summary>
    /// Gets the once.
    /// </summary>
    /// <value>The once.</value>
    public static SurveyPeriod Once => new(SurveyPeriodType.Once, null);

    /// <summary>
    /// Gets the quarterly.
    /// </summary>
    /// <value>The quarterly.</value>
    public static SurveyPeriod Quarterly => new(SurveyPeriodType.Quarterly, null);

    /// <summary>
    /// Gets the weekly.
    /// </summary>
    /// <value>The weekly.</value>
    public static SurveyPeriod Weekly => new(SurveyPeriodType.Weekly, null);

    /// <summary>
    /// Gets the yearly.
    /// </summary>
    /// <value>The yearly.</value>
    public static SurveyPeriod Yearly => new(SurveyPeriodType.Yearly, null);

    /// <summary>
    /// Gets the custom period.
    /// </summary>
    /// <value>The custom period.</value>
    [DataMember(Order = 2)]
    public string? CustomPeriod => customCronPeriod;

    /// <summary>
    /// Gets the type.
    /// </summary>
    /// <value>The type.</value>
    [DataMember(Order = 1)]
    public SurveyPeriodType Type => type;

    /// <summary>
    /// Customs the specified custom period.
    /// </summary>
    /// <param name="customPeriod">The custom period.</param>
    /// <returns>SurveyPeriod.</returns>
    public static SurveyPeriod Custom(string customPeriod) => new(SurveyPeriodType.Custom, customPeriod);
}
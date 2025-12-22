// <copyright file="SurveyEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents an abstract base class for survey events.
/// </summary>
/// <param name="Id">The unique identifier for the survey event.</param>
[PolymorphicSerialization]
public abstract partial record SurveyEvent(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier for the survey.
    /// </summary>
    /// <value>
    /// A string representing the unique aggregate identifier for the survey.
    /// </value>
    /// <remarks>
    /// This property uses the <see cref="SurveyDomainHelper.BuildSurveyDomainId"/> method to construct the aggregate ID.
    /// </remarks>
    public string DomainId
        => SurveyDomainHelper.BuildSurveyDomainId(Id);

    /// <summary>
    /// Gets the name of the aggregate for surveys.
    /// </summary>
    /// <value>
    /// A string representing the name of the survey aggregate.
    /// </value>
    /// <remarks>
    /// This property returns the constant value defined in <see cref="SurveyDomainHelper.SurveyDomainName"/>.
    /// </remarks>
    public string DomainName
        => SurveyDomainHelper.SurveyDomainName;
}
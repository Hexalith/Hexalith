// <copyright file="SurveyDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Provides helper methods and constants for the Survey domain.
/// </summary>
/// <remarks>
/// This static class contains utility methods and constants related to the Survey and SurveyCampaign aggregates.
/// It can be used across the domain layer to maintain consistency and reduce duplication.
/// The class offers methods for building aggregate IDs and provides constant names for the aggregates.
/// </remarks>
public static class SurveyDomainHelper
{
    /// <summary>
    /// Gets the name of the Survey aggregate.
    /// </summary>
    /// <remarks>
    /// This property returns the name of the Survey aggregate as a string.
    /// It uses the nameof operator to ensure consistency with the actual Survey class name.
    /// This can be useful for logging, serialization, or other scenarios where the aggregate name is needed.
    /// </remarks>
    public static string SurveyDomainName => nameof(Survey);

    /// <summary>
    /// Gets the name of the SurveyCampaign aggregate.
    /// </summary>
    /// <remarks>
    /// This property returns the name of the SurveyCampaign aggregate as a string.
    /// It uses the nameof operator to ensure consistency with the actual SurveyCampaign class name.
    /// This can be useful for logging, serialization, or other scenarios where the aggregate name is needed.
    /// </remarks>
    public static string SurveyCampaignDomainName => nameof(SurveyCampaign);

    /// <summary>
    /// Builds the aggregate ID for a Survey.
    /// </summary>
    /// <param name="id">The unique identifier for the Survey.</param>
    /// <returns>The aggregate ID for the Survey.</returns>
    /// <remarks>
    /// This method creates an aggregate ID for a Survey based on the provided unique identifier.
    /// In this implementation, it simply returns the input ID without modification.
    /// </remarks>
    public static string BuildSurveyAggregateId(string id) => id;

    /// <summary>
    /// Builds the aggregate ID for a Survey Campaign.
    /// </summary>
    /// <param name="id">The unique identifier for the Survey Campaign.</param>
    /// <returns>The aggregate ID for the Survey Campaign.</returns>
    /// <remarks>
    /// This method creates an aggregate ID for a Survey Campaign based on the provided unique identifier.
    /// In this implementation, it simply returns the input ID without modification.
    /// </remarks>
    public static string BuildSurveyCampaignAggregateId(string id) => id;
}
// <copyright file="SurveyCategory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Represents a category for surveys.
/// </summary>
[DataContract]
public record SurveyCategory(
    /// <summary>
    /// Gets the unique identifier for the survey category.
    /// </summary>
    string Id,

    /// <summary>
    /// Gets the name of the survey category.
    /// </summary>
    string Name);

// <copyright file="SurveyCategory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Represents a category for surveys.
/// </summary>
/// <param name="Id">The unique identifier for the survey category.</param>
/// <param name="Name">The name of the survey category.</param>
[DataContract]
public record SurveyCategory(
    string Id,
    string Name);
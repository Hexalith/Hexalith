// <copyright file="SurveyUser.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Represents a user participating in a survey.
/// </summary>
/// <param name="Id">The unique identifier of the survey user.</param>
/// <param name="Name">The name of the survey user.</param>
[DataContract]
public record SurveyUser(string Id, string Name);
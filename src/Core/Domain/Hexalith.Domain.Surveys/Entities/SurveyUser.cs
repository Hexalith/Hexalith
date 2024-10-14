﻿// <copyright file="SurveyUser.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Entities;

using System.Runtime.Serialization;

/// <summary>
/// Represents a user in the survey system.
/// </summary>
[DataContract]
public record SurveyUser(
    /// <summary>
    /// Gets the unique identifier for the user.
    /// </summary>
    string Id,

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    string Name);

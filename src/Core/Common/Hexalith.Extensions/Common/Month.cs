// <copyright file="Month.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

using System.Text.Json.Serialization;

/// <summary>
/// Enum Month.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Month
{
    /// <summary>
    /// The undefined.
    /// </summary>
    None = 0,

    /// <summary>
    /// The january.
    /// </summary>
    January = 1,

    /// <summary>
    /// The february.
    /// </summary>
    February = 2,

    /// <summary>
    /// The march.
    /// </summary>
    March = 3,

    /// <summary>
    /// The april.
    /// </summary>
    April = 4,

    /// <summary>
    /// The may.
    /// </summary>
    May = 5,

    /// <summary>
    /// The june.
    /// </summary>
    June = 6,

    /// <summary>
    /// The july.
    /// </summary>
    July = 7,

    /// <summary>
    /// The august.
    /// </summary>
    August = 8,

    /// <summary>
    /// The september.
    /// </summary>
    September = 9,

    /// <summary>
    /// The october.
    /// </summary>
    October = 10,

    /// <summary>
    /// The november.
    /// </summary>
    November = 11,

    /// <summary>
    /// The december.
    /// </summary>
    December = 12,
}
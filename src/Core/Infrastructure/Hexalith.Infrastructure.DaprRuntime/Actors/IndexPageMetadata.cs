// <copyright file="IndexPageMetadata.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

/// <summary>
/// Represents information about an index page.
/// </summary>
/// <param name="PageNumber">The page number.</param>
/// <param name="FirstValue">The first value.</param>
/// <param name="LastValue">The last value.</param>
/// <param name="ItemsCount">The number of items in the page.</param>
[DataContract]
public record IndexPageMetadata(
    [property: DataMember] int PageNumber,
    [property: DataMember] string FirstValue,
    [property: DataMember] string LastValue,
    [property: DataMember] int ItemsCount);
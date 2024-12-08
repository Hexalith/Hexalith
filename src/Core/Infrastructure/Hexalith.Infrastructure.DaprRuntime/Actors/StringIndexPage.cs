// <copyright file="StringIndexPage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

/// <summary>
/// Represents a page of string index data.
/// </summary>
/// <param name="Metadata">The metadata of the page.</param>
/// <param name="Data">The data of the page.</param>
/// <param name="ChildPages">The child pages of the page.</param>
[DataContract]
public record StringIndexPage(
    [property: DataMember] IndexPageMetadata Metadata,
    [property: DataMember] IEnumerable<string> Data,
    [property: DataMember] IDictionary<int, IndexPageMetadata> ChildPages);
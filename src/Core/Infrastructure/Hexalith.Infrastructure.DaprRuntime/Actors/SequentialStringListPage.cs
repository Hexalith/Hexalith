// <copyright file="SequentialStringListPage.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

/// <summary>
/// Represents a page of sequential strings.
/// </summary>
/// <param name="PageNumber">The page number.</param>
/// <param name="Data">The data.</param>
[DataContract]
public record SequentialStringListPage([property: DataMember] int PageNumber, [property: DataMember] IEnumerable<string> Data);
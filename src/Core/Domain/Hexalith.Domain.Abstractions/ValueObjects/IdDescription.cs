// <copyright file="IdDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents an identifier-description pair.
/// </summary>
/// <remarks>
/// This record is used to store and retrieve information about an entity
/// using its identifier and description.
/// </remarks>
[DataContract]
public record IdDescription(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Description,
    [property: DataMember(Order = 3)] bool Disabled) : IIdDescription
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdDescription"/> class.
    /// </summary>
    public IdDescription()
        : this(string.Empty, string.Empty, false)
    {
    }
}
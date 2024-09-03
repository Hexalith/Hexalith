// <copyright file="IPolymorphicSerializationMapper.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerialization;

using System;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Interface for serialization mapper.
/// </summary>
public interface IPolymorphicSerializationMapper
{
    /// <summary>
    /// Gets the base type.
    /// </summary>
    Type Base { get; }

    /// <summary>
    /// Gets the Json derived type.
    /// </summary>
    JsonDerivedType JsonDerivedType { get; }
}
// <copyright file="DimensionValue.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents a dimension value.
/// </summary>
[DataContract]
public record DimensionValue(
    /// <summary>
    /// Gets or sets the type of the dimension.
    /// </summary>
    [property:DataMember(Order = 1)]
    string Type,

    /// <summary>
    /// Gets or sets the value of the dimension.
    /// </summary>
    [property:DataMember(Order = 2)]
    string Value,

    /// <summary>
    /// Gets or sets the name of the dimension.
    /// </summary>
    [property:DataMember(Order = 3)]
    string Name)
{
}
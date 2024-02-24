// <copyright file="ProductDimension.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Products.ValueObjects;

using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Represents a product dimension.
/// </summary>
[DataContract]
[Serializable]
public class ProductDimension
{
    /// <summary>
    /// Gets or sets the name of the product dimension.
    /// </summary>
    /// <value>
    /// The name of the product dimension.
    /// </value>
    [DataMember(Order = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the values of the product dimension.
    /// </summary>
    /// <value>
    /// The values of the product dimension.
    /// </value>
    [DataMember(Order = 2)]
    public IEnumerable<string> Values { get; set; } = [];
}
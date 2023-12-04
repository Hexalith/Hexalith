// <copyright file="ProductIdentifier.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ValueObjets;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class ProductIdentifier.
/// </summary>
[DataContract]
[Serializable]
public class ProductIdentifier
{
    /// <summary>
    /// Gets or sets the barcode.
    /// </summary>
    /// <value>The barcode.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets the color identifier.
    /// </summary>
    /// <value>The color identifier.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public string? ColorId { get; set; }

    /// <summary>
    /// Gets or sets the configuration identifier.
    /// </summary>
    /// <value>The configuration identifier.</value>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    public string? ConfigurationId { get; set; }

    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    /// <value>The item identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string? ItemId { get; set; }

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>The quantity.</value>
    [DataMember(Order = 9)]
    [JsonPropertyOrder(9)]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Gets or sets the size identifier.
    /// </summary>
    /// <value>The size identifier.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string? SizeId { get; set; }

    /// <summary>
    /// Gets or sets the style identifier.
    /// </summary>
    /// <value>The style identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string? StyleId { get; set; }

    /// <summary>
    /// Gets or sets the system identifier.
    /// </summary>
    /// <value>The system identifier.</value>
    [DataMember(Order = 8)]
    [JsonPropertyOrder(8)]
    public string? SystemId { get; set; }

    /// <summary>
    /// Gets or sets the unit identifier.
    /// </summary>
    /// <value>The unit identifier.</value>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string? UnitId { get; set; }

    /// <summary>
    /// Gets or sets the version identifier.
    /// </summary>
    /// <value>The version identifier.</value>
    [DataMember(Order = 7)]
    [JsonPropertyOrder(7)]
    public string? VersionId { get; set; }
}
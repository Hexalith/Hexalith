// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="ProductDimensionsChanged.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Events namespace.
/// </summary>
namespace Hexalith.Domain.Products.Events;

using System.Runtime.Serialization;

using Hexalith.Domain.Products.ValueObjects;
using Hexalith.Extensions;

/// <summary>
/// Class ProductConversionInformationChanged.
/// Implements the <see cref="ProductEvent" />.
/// </summary>
/// <seealso cref="ProductEvent" />
[DataContract]
[Serializable]
public class ProductDimensionsChanged : ProductEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDimensionsChanged" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="productDimensions">The product dimensions.</param>
    /// <param name="excludedDimensionCombinaisons">The excluded dimension combinaisons.</param>
    public ProductDimensionsChanged(
        string partitionId,
        string originId,
        string id,
        IEnumerable<ProductDimension>? productDimensions,
        IEnumerable<IEnumerable<string>>? excludedDimensionCombinaisons)
        : base(partitionId, originId, id)
    {
        ProductDimensions = productDimensions;
        ExcludedDimensionCombinaisons = excludedDimensionCombinaisons;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductDimensionsChanged" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ProductDimensionsChanged()
    {
    }

    /// <summary>
    /// Gets or sets the excluded dimension combinaisons.
    /// </summary>
    /// <value>The excluded dimension combinaisons.</value>
    [DataMember(Order = 21)]
    public IEnumerable<IEnumerable<string>>? ExcludedDimensionCombinaisons { get; set; }

    /// <summary>
    /// Gets or sets the product dimensions.
    /// </summary>
    /// <value>The product dimensions.</value>
    [DataMember(Order = 20)]
    public IEnumerable<ProductDimension>? ProductDimensions { get; set; }
}
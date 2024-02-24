// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesLineTax.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.ValueObjets;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;
using Hexalith.Extensions.Common;

/// <summary>
/// Class SalesLineItem.
/// </summary>
[DataContract]
[Serializable]
public class SalesLineTax : IEquatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineTax" /> class.
    /// </summary>
    /// <param name="taxId">The tax identifier.</param>
    /// <param name="name">The name.</param>
    /// <param name="amount">The amount.</param>
    [JsonConstructor]
    public SalesLineTax(string taxId, string name, decimal amount)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        TaxId = taxId;
        Amount = amount;
        Name = name;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineTax"/> class.
    /// </summary>
    /// <param name="lineTax">The line tax.</param>
    public SalesLineTax(SalesLineTax lineTax)
        : this(
              (lineTax ?? throw new ArgumentNullException(nameof(lineTax))).TaxId,
              lineTax.Name,
              lineTax.Amount)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineTax" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    [ExcludeFromCodeCoverage]
    public SalesLineTax() => TaxId = Name = string.Empty;

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public decimal Amount
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string Name
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the tax identifier.
    /// </summary>
    /// <value>The tax identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string TaxId
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <inheritdoc/>
    public IEnumerable<object?> GetEqualityComponents()
        => [TaxId, Name, Amount];
}
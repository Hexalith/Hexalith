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
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class SalesLineItem.
/// </summary>
[DataContract]
[Serializable]
public class SalesLineTax
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
        TaxId = taxId;
        Amount = amount;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineTax" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SalesLineTax() => TaxId = Name = string.Empty;

    /// <summary>
    /// Gets or sets the amount.
    /// </summary>
    /// <value>The amount.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the tax identifier.
    /// </summary>
    /// <value>The tax identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string TaxId { get; set; }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(SalesLineTax? a, SalesLineTax? b)
    {
        return a is null
            ? b is null
            : a == b ||
                (a.TaxId == b?.TaxId &&
                a.Name == b?.Name &&
                a.Amount == b?.Amount);
    }
}
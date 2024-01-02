// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesInvoiceLineState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Entities;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions;

/// <summary>
/// Class Contact.
/// </summary>
[DataContract]
[Serializable]
public class SalesInvoiceLineState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceLineState"/> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="item">The item.</param>
    /// <param name="origin">The origin.</param>
    /// <param name="taxes">The taxes.</param>
    [JsonConstructor]
    public SalesInvoiceLineState(string id, SalesLineItem item, SalesLineOrigin origin, IEnumerable<SalesLineTax> taxes)
    {
        Id = id;
        Item = item;
        Origin = origin;
        Taxes = taxes;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceLineState" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SalesInvoiceLineState()
    {
        Item = new SalesLineItem();
        Origin = new SalesLineOrigin();
        Taxes = Array.Empty<SalesLineTax>();
        Id = string.Empty;
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    /// <value>The item.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public SalesLineItem Item { get; set; }

    /// <summary>
    /// Gets or sets the origin.
    /// </summary>
    /// <value>The origin.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public SalesLineOrigin Origin { get; set; }

    /// <summary>
    /// Gets or sets the taxes.
    /// </summary>
    /// <value>The taxes.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public IEnumerable<SalesLineTax> Taxes { get; set; } = new List<SalesLineTax>();

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(SalesInvoiceLineState? a, SalesInvoiceLineState? b)
    {
        return a is null
            ? b is null
            : a == b ||
                (a.Id == b?.Id &&
                SalesLineItem.AreSame(a.Item, b?.Item) &&
                SalesLineOrigin.AreSame(a.Origin, b?.Origin) &&
                AreSameTaxes(a.Taxes, b?.Taxes));
    }

    private static bool AreSameTaxes(IEnumerable<SalesLineTax> taxes1, IEnumerable<SalesLineTax>? taxes2)
    {
        if (taxes1 is null)
        {
            return taxes2 is null;
        }
        else if (taxes2 is null)
        {
            return false;
        }
        else
        {
            SalesLineTax[] taxes1Array = taxes1.ToArray();
            SalesLineTax[] taxes2Array = taxes2.ToArray();
            if (taxes1Array.Length != taxes2Array.Length)
            {
                return false;
            }

            for (int i = 0; i < taxes1Array.Length; i++)
            {
                if (!SalesLineTax.AreSame(taxes1Array[i], taxes2Array[i]))
                {
                    return false;
                }
            }
        }

        return true;
    }
}
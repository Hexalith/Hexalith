// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="SalesInvoiceLine.cs" company="Fiveforty SAS Paris France">
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
public class SalesInvoiceLine : IEquatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceLine" /> class.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="item">The item.</param>
    /// <param name="origin">The origin.</param>
    /// <param name="taxes">The taxes.</param>
    [JsonConstructor]
    public SalesInvoiceLine(string id, SalesLineItem item, SalesLineOrigin origin, IEnumerable<SalesLineTax> taxes)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        Id = id;
        Item = new SalesLineItem(item);
        Origin = new SalesLineOrigin(origin);
        Taxes = taxes.Select(p => new SalesLineTax(p)).ToList();
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceLine" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    [ExcludeFromCodeCoverage]
    public SalesInvoiceLine()
    {
        Item = new SalesLineItem();
        Origin = new SalesLineOrigin();
        Taxes = [];
        Id = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceLine"/> class.
    /// </summary>
    /// <param name="line">The line.</param>
    public SalesInvoiceLine(SalesInvoiceLine line)
        : this(
              (line ?? throw new ArgumentNullException(nameof(line))).Id,
              line.Item,
              line.Origin,
              line.Taxes)
    {
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Id
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    /// <value>The item.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public SalesLineItem Item
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the origin.
    /// </summary>
    /// <value>The origin.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public SalesLineOrigin Origin
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <summary>
    /// Gets or sets the taxes.
    /// </summary>
    /// <value>The taxes.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public IEnumerable<SalesLineTax> Taxes
    {
        get;
        [Obsolete(DefaultLabels.ForSerializationOnly, false)]
        set;
    }

    /// <inheritdoc/>
    public IEnumerable<object?> GetEqualityComponents()
        => [Id, Item, Origin, Taxes];
}
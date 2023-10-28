// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-12-2023
// ***********************************************************************
// <copyright file="ItemBarcode.cs" company="Fiveforty SAS Paris France">
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
/// Class Contact.
/// </summary>
[DataContract]
public class ItemBarcode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ItemBarcode" /> class.
    /// </summary>
    /// <param name="code">The bar code.</param>
    /// <param name="isDefault">if set to <c>true</c> [is default].</param>
    [JsonConstructor]
    public ItemBarcode(
        string code,
        bool isDefault)
    {
        Code = code;
        IsDefault = isDefault;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ItemBarcode" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ItemBarcode()
    {
        Code = string.Empty;
        IsDefault = false;
    }

    /// <summary>
    /// Gets or sets the postal address.
    /// </summary>
    /// <value>The postal address.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is default.
    /// </summary>
    /// <value><c>null</c> if [is default] contains no value, <c>true</c> if [is default]; otherwise, <c>false</c>.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public bool? IsDefault { get; set; }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(ItemBarcode? a, ItemBarcode? b)
    {
        return a is null
            ? b is null
            : a == b || (
                a.Code == b?.Code &&
                a.IsDefault == b?.IsDefault);
    }
}
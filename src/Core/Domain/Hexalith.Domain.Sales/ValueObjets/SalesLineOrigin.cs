// ***********************************************************************
// Assembly         : Hexalith.Domain.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-02-2024
// ***********************************************************************
// <copyright file="SalesLineOrigin.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Domain.ValueObjets;

using System;
using System.Collections.Generic;
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
public class SalesLineOrigin : IEquatableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineOrigin" /> class.
    /// </summary>
    /// <param name="locationId">The location identifier.</param>
    /// <param name="vendorId">The vendor identifier.</param>
    [JsonConstructor]
    public SalesLineOrigin(string locationId, string vendorId)
    {
        LocationId = locationId;
        VendorId = vendorId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesLineOrigin" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    [ExcludeFromCodeCoverage]
    public SalesLineOrigin() => LocationId = VendorId = string.Empty;

    /// <summary>
    /// Gets or sets the location identifier.
    /// </summary>
    /// <value>The location identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string LocationId { get; set; }

    /// <summary>
    /// Gets or sets the vendor identifier.
    /// </summary>
    /// <value>The vendor identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string VendorId { get; set; }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(SalesLineOrigin? a, SalesLineOrigin? b)
    {
        return a is null
            ? b is null
            : a == b ||
                (a.VendorId == b?.VendorId &&
                a.LocationId == b?.LocationId);
    }

    /// <inheritdoc/>
    public IEnumerable<object?> GetEqualityComponents()
        => new object?[] { LocationId, VendorId };
}
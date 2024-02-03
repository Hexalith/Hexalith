// <copyright file="GeoLocation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ValueObjets;

using System.Runtime.Serialization;

/// <summary>
/// Represents a geographic location with latitude and longitude coordinates.
/// </summary>
[Serializable]
[DataContract]
public class GeoLocation
{
    /// <summary>
    /// Gets or sets the latitude coordinate of the location.
    /// </summary>
    [DataMember]
    public double Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude coordinate of the location.
    /// </summary>
    [DataMember]
    public double Longitude { get; set; }
}
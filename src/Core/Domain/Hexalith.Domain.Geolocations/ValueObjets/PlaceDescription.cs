// <copyright file="PlaceDescription.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.ValueObjets;

using System.Runtime.Serialization;

/// <summary>
/// Class GooglePlace.
/// </summary>
[Serializable]
[DataContract]
public class PlaceDescription
{
    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    [DataMember]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember]
    public string? Id { get; set; }
}
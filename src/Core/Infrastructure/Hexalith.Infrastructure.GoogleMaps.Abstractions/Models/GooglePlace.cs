// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.GoogleMaps.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-20-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-21-2024
// ***********************************************************************
// <copyright file="GooglePlace.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.GoogleMaps.Models;

using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Class GooglePlace.
/// </summary>
[Serializable]
[DataContract]
public class GooglePlace
{
    /// <summary>
    /// Gets or sets the postal address.
    /// </summary>
    /// <value>The postal address.</value>
    [DataMember]
    public string? Address { get; set; }

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
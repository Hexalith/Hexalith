// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-29-2023
// ***********************************************************************
// <copyright file="ExternalReference.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ValueObjets;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class ExternalReference.
/// </summary>
[DataContract]
[Serializable]
public class ExternalReference
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalReference" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ExternalReference() => SystemId = ExternalId = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalReference" /> class.
    /// </summary>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    [JsonConstructor]
    public ExternalReference(string systemId, string externalId)
    {
        SystemId = systemId;
        ExternalId = externalId;
    }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [JsonPropertyOrder(2)]
    [DataMember(Order = 2)]
    public string ExternalId { get; set; }

    /// <summary>
    /// Gets or sets the system.
    /// </summary>
    /// <value>The system.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string SystemId { get; set; }
}
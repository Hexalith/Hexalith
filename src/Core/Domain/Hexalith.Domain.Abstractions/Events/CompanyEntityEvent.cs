// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="CompanyEntityEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class CompanyEntityEvent.
/// Implements the <see cref="CompanyEntityEvent" />.
/// </summary>
/// <seealso cref="CompanyEntityEvent" />
[DataContract]
[Serializable]
public abstract class CompanyEntityEvent : CommonEntityEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityEvent"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected CompanyEntityEvent(string partitionId, string companyId, string originId, string id)
        : base(partitionId, originId, id)
            => CompanyId = companyId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityEvent"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected CompanyEntityEvent() => CompanyId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string CompanyId { get; set; }
}
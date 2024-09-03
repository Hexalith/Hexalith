// ***********************************************************************
// Assembly         : Hexalith.Application.Organizations
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ByCompanyProjection.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.Organizations.Projections;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class ByCompanyProjection.
/// Implements the <see cref="Hexalith.Application.Organizations.Projections.ByPartitionProjection" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Organizations.Projections.ByPartitionProjection" />
public abstract class ByCompanyProjection : ByPartitionProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ByCompanyProjection"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    [JsonConstructor]
    protected ByCompanyProjection(string partitionId, string companyId)
        : base(partitionId)
            => CompanyId = companyId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByCompanyProjection"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected ByCompanyProjection() => CompanyId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string CompanyId { get; set; }
}
﻿// ***********************************************************************
// Assembly         : Hexalith.Application.Organizations
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ByPartitionProjection.cs" company="Jérôme Piquot">
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
/// Class ByPartitionProjection.
/// </summary>
public abstract class ByPartitionProjection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ByPartitionProjection"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    [JsonConstructor]
    protected ByPartitionProjection(string partitionId) => PartitionId = partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="ByPartitionProjection"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected ByPartitionProjection() => PartitionId = string.Empty;

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; set; }
}
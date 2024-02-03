// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="PartitionedCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Organizations.Commands;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Extensions;

/// <summary>
/// Class PartitionedCommand.
/// Implements the <see cref="BaseCommand" />.
/// </summary>
/// <seealso cref="BaseCommand" />
[DataContract]
[Serializable]
public abstract class PartitionedCommand : BaseCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    [JsonConstructor]
    protected PartitionedCommand(string partitionId) => PartitionId = partitionId;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartitionedCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected PartitionedCommand() => PartitionId = string.Empty;

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => DefaultAggregateName() + Separator + PartitionId;
}
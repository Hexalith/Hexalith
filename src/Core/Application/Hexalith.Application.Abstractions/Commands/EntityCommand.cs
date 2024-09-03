// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-21-2023
// ***********************************************************************
// <copyright file="EntityCommand.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class EntityCommand.
/// Implements the <see cref="Domain.Commands.PartitionedCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.PartitionedCommand" />
[DataContract]
public abstract class EntityCommand : BaseCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected EntityCommand(string partitionId, string originId, string id)
    {
        PartitionId = partitionId;
        OriginId = originId;
        Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected EntityCommand() => PartitionId = OriginId = Id = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string OriginId { get; set; }

    /// <summary>
    /// Gets or sets the partition identifier.
    /// </summary>
    /// <value>The partition identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string PartitionId { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId()
        => base.DefaultAggregateId() + Separator + OriginId + Separator + Id;
}
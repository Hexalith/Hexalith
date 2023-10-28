// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="CompanyCommand.cs" company="Fiveforty SAS Paris France">
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

using Hexalith.Extensions;

/// <summary>
/// Class CompanyCommand.
/// Implements the <see cref="Domain.Commands.PartitionedCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.PartitionedCommand" />
[DataContract]
public abstract class CompanyCommand : PartitionedCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyCommand" /> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    [JsonConstructor]
    protected CompanyCommand(string partitionId, string companyId)
        : base(partitionId)
            => CompanyId = companyId;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyCommand" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    protected CompanyCommand() => CompanyId = string.Empty;

    /// <summary>
    /// Gets or sets the company identifier.
    /// </summary>
    /// <value>The company identifier.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string CompanyId { get; set; }
}
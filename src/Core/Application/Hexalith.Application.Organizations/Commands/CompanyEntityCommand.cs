// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-16-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="CompanyEntityCommand.cs" company="Fiveforty SAS Paris France">
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

/// <summary>
/// Class CompanyEntityCommand.
/// Implements the <see cref="Domain.Commands.CompanyCommand" />.
/// </summary>
/// <seealso cref="Domain.Commands.CompanyCommand" />
[DataContract]
public abstract class CompanyEntityCommand : CompanyCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityCommand"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected CompanyEntityCommand(string partitionId, string companyId, string id)
        : base(partitionId, companyId)
            => Id = id;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyEntityCommand"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected CompanyEntityCommand() => Id = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string Id { get; set; }
}
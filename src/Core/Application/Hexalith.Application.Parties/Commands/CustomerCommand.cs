// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="CustomerCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;

/// <summary>
/// Class CustomerEvent.
/// Implements the <see cref="BaseEvent" />.
/// </summary>
/// <seealso cref="BaseEvent" />
[DataContract]
public abstract class CustomerCommand : BaseCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerCommand"/> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    [JsonConstructor]
    protected CustomerCommand(string companyId, string id)
    {
        Id = id;
        CompanyId = companyId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerCommand" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    protected CustomerCommand() => CompanyId = Id = string.Empty;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string Id { get; set; }

    /// <inheritdoc/>
    protected override string DefaultAggregateId() => Customer.GetAggregateId(CompanyId, Id);

    /// <inheritdoc/>
    protected override string DefaultAggregateName() => Customer.GetAggregateName();
}
// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-30-2023
// ***********************************************************************
// <copyright file="CustomerRegistered.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class SalesInvoiceRegistered.
/// Implements the <see cref="Hexalith.Domain.Events.SalesInvoiceEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.SalesInvoiceEvent" />
[DataContract]
[Serializable]
public class SalesInvoiceIssued : SalesInvoiceEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceIssued"/> class.
    /// </summary>
    /// <param name="partitionId"></param>
    /// <param name="companyId"></param>
    /// <param name="originId"></param>
    /// <param name="id"></param>
    /// <param name="createdDate"></param>
    /// <param name=""></param>
    [JsonConstructor]
    public SalesInvoiceIssued(
        string partitionId,
        string companyId,
        string originId,
        string id,
        DateTimeOffset createdDate)
        : base(partitionId, companyId, originId, id) => CreatedDate = createdDate;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesInvoiceIssued"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SalesInvoiceIssued()
    {
    }

    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public DateTimeOffset CreatedDate { get; set; }
}
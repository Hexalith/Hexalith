// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="UnsetCustomerIntercompanyDirectDelivery.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Commands;

using System.Runtime.Serialization;

/// <summary>
/// Class SetCustomerIntercompanyDeliveryToIndirect.
/// Implements the <see cref="Hexalith.Application.Parties.Commands.CustomerCommand" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Parties.Commands.CustomerCommand" />
[DataContract]
public class UnsetCustomerIntercompanyDirectDelivery : CustomerCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnsetCustomerIntercompanyDirectDelivery"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    public UnsetCustomerIntercompanyDirectDelivery(
        string partitionId,
        string companyId,
        string id)
        : base(partitionId, companyId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnsetCustomerIntercompanyDirectDelivery" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public UnsetCustomerIntercompanyDirectDelivery()
    {
    }
}
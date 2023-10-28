// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-03-2023
// ***********************************************************************
// <copyright file="SetCustomerIntercompanyDirectDelivery.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Parties.Commands;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class SetCustomerIntercompanyDeliveryToDirect.
/// Implements the <see cref="Application.Commands.CustomerCommand" />.
/// </summary>
/// <seealso cref="Application.Commands.CustomerCommand" />
[DataContract]
public class SetCustomerIntercompanyDirectDelivery : CustomerCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetCustomerIntercompanyDirectDelivery"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    public SetCustomerIntercompanyDirectDelivery(
        string partitionId,
        string companyId,
        string id)
        : base(partitionId, companyId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetCustomerIntercompanyDirectDelivery"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public SetCustomerIntercompanyDirectDelivery()
    {
    }
}
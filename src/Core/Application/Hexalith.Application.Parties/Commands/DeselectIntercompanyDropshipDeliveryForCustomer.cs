// ***********************************************************************
// Assembly         : Hexalith.Application.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-04-2023
// ***********************************************************************
// <copyright file="DeselectIntercompanyDropshipDeliveryForCustomer.cs" company="Fiveforty SAS Paris France">
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
/// Class SetCustomerIntercompanyDeliveryToIndirect.
/// Implements the <see cref="Hexalith.Application.Parties.Commands.CustomerCommand" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Parties.Commands.CustomerCommand" />
[DataContract]
[Serializable]
public class DeselectIntercompanyDropshipDeliveryForCustomer : CustomerCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeselectIntercompanyDropshipDeliveryForCustomer"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="originId">The origin identifier.</param>
    /// <param name="id">The identifier.</param>
    public DeselectIntercompanyDropshipDeliveryForCustomer(
        string partitionId,
        string companyId,
        string originId,
        string id)
        : base(partitionId, companyId, originId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeselectIntercompanyDropshipDeliveryForCustomer" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public DeselectIntercompanyDropshipDeliveryForCustomer()
    {
    }
}
// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-03-2023
// ***********************************************************************
// <copyright file="CustomerIntercompanyDeliveryTypeSetToDirect.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class CustomerIntercompanyDeliveryTypeSetToDirect.
/// Implements the <see cref="Hexalith.Domain.Events.CustomerEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.CustomerEvent" />
[DataContract]
public class CustomerIntercompanyDeliveryTypeSetToDirect : CustomerEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerIntercompanyDeliveryTypeSetToDirect"/> class.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    public CustomerIntercompanyDeliveryTypeSetToDirect(
        string partitionId,
        string companyId,
        string id)
        : base(partitionId, companyId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerIntercompanyDeliveryTypeSetToDirect"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public CustomerIntercompanyDeliveryTypeSetToDirect()
    {
    }
}
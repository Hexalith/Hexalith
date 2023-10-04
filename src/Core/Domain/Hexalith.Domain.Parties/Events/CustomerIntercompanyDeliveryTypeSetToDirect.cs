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
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="date">The date.</param>
    public CustomerIntercompanyDeliveryTypeSetToDirect(
        string companyId,
        string id)
        : base(companyId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerIntercompanyDeliveryTypeSetToDirect"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public CustomerIntercompanyDeliveryTypeSetToDirect()
    {
    }
}
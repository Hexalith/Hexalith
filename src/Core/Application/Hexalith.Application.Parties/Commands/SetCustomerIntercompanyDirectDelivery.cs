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

/// <summary>
/// Class SetCustomerIntercompanyDeliveryToDirect.
/// Implements the <see cref="Application.Commands.CustomerCommand" />.
/// </summary>
/// <seealso cref="Application.Commands.CustomerCommand" />
[DataContract]
public class SetCustomerIntercompanyDeliveryToDirect : CustomerCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SetCustomerIntercompanyDeliveryToDirect"/> class.
    /// </summary>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="date">The date.</param>
    public SetCustomerIntercompanyDeliveryToDirect(
        string companyId,
        string id)
        : base(companyId, id)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SetCustomerIntercompanyDeliveryToDirect"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public SetCustomerIntercompanyDeliveryToDirect()
    {
    }
}
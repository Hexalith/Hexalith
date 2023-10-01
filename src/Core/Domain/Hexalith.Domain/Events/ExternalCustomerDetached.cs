// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 08-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-21-2023
// ***********************************************************************
// <copyright file="ExternalCustomerDetached.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.Events;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class ExternalCustomerDetached.
/// Implements the <see cref="Hexalith.Domain.Events.ExternalCustomerEvent" />.
/// </summary>
/// <seealso cref="Hexalith.Domain.Events.ExternalCustomerEvent" />
[DataContract]
public class ExternalCustomerDetached : ExternalCustomerEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomerDetached" /> class.
    /// </summary>
    /// <param name="system">The system.</param>
    /// <param name="externalId">The external identifier.</param>
    [JsonConstructor]
    public ExternalCustomerDetached(string system, string externalId)
        : base(system, externalId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalCustomerDetached" /> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public ExternalCustomerDetached()
    {
    }
}
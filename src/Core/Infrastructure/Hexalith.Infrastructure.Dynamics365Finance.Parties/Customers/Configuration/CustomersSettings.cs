// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 12-22-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-22-2023
// ***********************************************************************
// <copyright file="CustomersSettings.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Configuration;

using System.Runtime.Serialization;

/// <summary>
/// Class PartiesSettings.
/// </summary>
[Serializable]
[DataContract]
public class CustomersSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether [receive customers from erp enabled].
    /// </summary>
    /// <value><c>true</c> if [receive customers from erp enabled]; otherwise, <c>false</c>.</value>
    [DataMember(Order = 2)]
    public bool ReceiveCustomersFromErpEnabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [send customers to erp enabled].
    /// </summary>
    /// <value><c>true</c> if [send customers to erp enabled]; otherwise, <c>false</c>.</value>
    [DataMember(Order = 1)]
    public bool SendCustomersToErpEnabled { get; set; }
}
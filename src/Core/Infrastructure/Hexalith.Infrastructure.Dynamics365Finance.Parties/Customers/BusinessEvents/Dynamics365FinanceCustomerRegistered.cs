// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 08-28-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerRegistered.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using System.Runtime.Serialization;

/// <summary>
/// This is the base class for logistics partner catalog events from Dynamics 365 for finance and operations.
/// </summary>
[DataContract]
public class Dynamics365FinanceCustomerRegistered : Dynamics365FinanceCustomerInformationBusinessEvent
{
    /// <inheritdoc/>
    protected override string DefaultTypeName() => "FFYCustomerRegisteredBusinessEvent";
}
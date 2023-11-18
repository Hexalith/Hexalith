// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 11-18-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 11-18-2023
// ***********************************************************************
// <copyright file="CustomerInformationChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.IntegrationEvents;

using Hexalith.Application.Events;
using Hexalith.Domain.Events;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Entities;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="IntegrationEventHandler{CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="IntegrationEventHandler{CustomerInformationChanged}" />
public class CustomerInformationChangedHandler : CustomerChangedHandler<CustomerInformationChanged>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerInformationChangedHandler"/> class.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    public CustomerInformationChangedHandler(IDynamics365FinanceClient<CustomerV3> customerService)
        : base(customerService)
    {
    }
}
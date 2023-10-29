// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="CustomerInformationChangedHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.Parties.IntegrationEvents;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Events;
using Hexalith.Domain.Events;

/// <summary>
/// Class CustomerInformationChangedHandler.
/// Implements the <see cref="Hexalith.Application.Events.IntegrationEventHandler{Hexalith.Domain.Events.CustomerInformationChanged}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Events.IntegrationEventHandler{Hexalith.Domain.Events.CustomerInformationChanged}" />
internal class CustomerInformationChangedHandler : IntegrationEventHandler<CustomerInformationChanged>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseCommand>> ApplyAsync(CustomerInformationChanged @event, CancellationToken cancellationToken) => throw new NotImplementedException();
}
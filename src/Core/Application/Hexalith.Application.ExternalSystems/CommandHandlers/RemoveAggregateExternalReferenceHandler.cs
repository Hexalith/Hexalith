// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="RemoveAggregateExternalReferenceHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.ExternalSystems.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class RemoveAggregateExternalReferenceHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.ExternalSystems.Commands.RemoveAggregateExternalReference}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.ExternalSystems.Commands.RemoveAggregateExternalReference}" />
public class RemoveAggregateExternalReferenceHandler : CommandHandler<RemoveAggregateExternalReference>
{
    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> DoAsync(RemoveAggregateExternalReference command, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<BaseMessage>>(new AggregateExternalReferenceRemoved(
                command.ReferenceAggregateId,
                command.SystemId,
                command.ExternalId)
                .IntoArray<BaseMessage>());
    }

    /// <inheritdoc/>
    public override Task<IEnumerable<BaseMessage>> UndoAsync(RemoveAggregateExternalReference command, CancellationToken cancellationToken)
        => throw new NotSupportedException();
}
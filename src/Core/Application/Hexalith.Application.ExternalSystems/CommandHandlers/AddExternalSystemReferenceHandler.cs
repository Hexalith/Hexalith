// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="AddExternalSystemReferenceHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.ExternalSystems.CommandHandlers;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Events;
using Hexalith.Domain.Messages;

/// <summary>
/// Class MapExternalSystemReferenceHandler.
/// Implements the <see cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.ExternalSystems.Commands.AddExternalSystemReference}" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.CommandHandler{Hexalith.Application.ExternalSystems.Commands.AddExternalSystemReference}" />
public class AddExternalSystemReferenceHandler : CommandHandler<AddExternalSystemReference>
{
    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> DoAsync([NotNull] AddExternalSystemReference command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        return aggregate is not null and ExternalSystemReference external && external.ReferenceAggregateId == command.ReferenceAggregateId
            ? []
            : await Task.FromResult<IEnumerable<BaseMessage>>([new ExternalSystemReferenceAdded(
            command.PartitionId,
            command.CompanyId,
            command.SystemId,
            command.ReferenceAggregateName,
            command.ExternalId,
            command.ReferenceAggregateId)]).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task<IEnumerable<BaseMessage>> UndoAsync(AddExternalSystemReference command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        await Task.CompletedTask.ConfigureAwait(false);
        throw new NotSupportedException();
    }
}
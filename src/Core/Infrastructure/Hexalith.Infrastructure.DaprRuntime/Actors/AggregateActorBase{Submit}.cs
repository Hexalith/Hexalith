// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.Sales
// Author           : Jérôme Piquot
// Created          : 01-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-03-2024
// ***********************************************************************
// <copyright file="AggregateActorBase{Submit}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// The aggregate manager actor class.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// Implements the <see cref="IAggregateActor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Sales.Actors.AggregateActorBase" />
/// <seealso cref="IAggregateActor" />
public abstract partial class AggregateActorBase
{
    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ActorCommandEnvelope envelope)
    {
        CancellationToken cancellationToken = CancellationToken.None;
        ArgumentNullException.ThrowIfNull(envelope);
        Application.Commands.BaseCommand[] commands = envelope.Commands.ToArray();
        Application.Metadatas.BaseMetadata[] metadatas = envelope.Metadatas.ToArray();
        if (commands.Length != metadatas.Length)
        {
            throw new InvalidOperationException($"Invalid commands envelope submitted to {Host.ActorTypeInfo.ActorTypeName}/{Id}. Command and Metadata arrays must have the same number of elements.");
        }

        if (commands.Length == 0)
        {
            LogNoCommandsToSubmitWarning(Logger, Id.ToString(), Host.ActorTypeInfo.ActorTypeName);
            return;
        }

        List<CommandState> commandStates = [];
        for (int i = 0; i < commands.Length; i++)
        {
            BaseCommand command = commands[i];
            BaseMetadata metadata = metadatas[i];
            if (GetAggregateActorName(command.AggregateName) != Host.ActorTypeInfo.ActorTypeName)
            {
                throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate name : {command.AggregateName}.");
            }

            if (command.AggregateId != Id.ToString())
            {
                throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate id : {command.AggregateId}.");
            }

            CommandState commandState = new(_dateTimeService.UtcNow, command, metadata);
            commandStates.Add(commandState);
            LogAcceptedCommandInformation(
                Logger,
                command.TypeName,
                metadata.Message.Id,
                command.AggregateName,
                command.AggregateId);
        }

        AggregateActorState state = await GetAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        state.CommandCount = await CommandStore
            .AddAsync(commandStates, state.CommandCount, cancellationToken)
            .ConfigureAwait(false);

        await SetProcessCallbackAsync(cancellationToken)
           .ConfigureAwait(false);
        await SetPublishCallbackAsync(cancellationToken)
           .ConfigureAwait(false);
        await SaveAggregateStateAsync(cancellationToken)
            .ConfigureAwait(false);
        await SaveStateAsync().ConfigureAwait(false);
    }
}
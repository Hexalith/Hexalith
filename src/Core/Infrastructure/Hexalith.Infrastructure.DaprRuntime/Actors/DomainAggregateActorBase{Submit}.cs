// <copyright file="DomainAggregateActorBase{Submit}.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

public abstract partial class DomainAggregateActorBase
{
    /// <inheritdoc/>
    public async Task SubmitCommandAsync(ActorMessageEnvelope envelope)
    {
        CancellationToken cancellationToken = CancellationToken.None;
        ArgumentNullException.ThrowIfNull(envelope);

        List<MessageState> commandStates = [];
        object command = envelope.Message;
        Metadata metadata = envelope.Metadata;
        if (GetAggregateActorName(metadata.Message.Aggregate.Name) != Host.ActorTypeInfo.ActorTypeName)
        {
            throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate name : {metadata.Message.Aggregate.Name}.");
        }

        if (metadata.Message.Aggregate.Id != Id.ToString())
        {
            throw new InvalidOperationException($"Submitted command to {Host.ActorTypeInfo.ActorTypeName}/{Id} has an invalid aggregate id : {metadata.Message.Aggregate.Id}.");
        }

        MessageState commandState = new(command, metadata);
        commandStates.Add(commandState);
        LogAcceptedCommandInformation(
            Logger,
            metadata.Message.Name,
            metadata.Message.Id,
            metadata.Message.Aggregate.Name,
            metadata.Message.Aggregate.Id);

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
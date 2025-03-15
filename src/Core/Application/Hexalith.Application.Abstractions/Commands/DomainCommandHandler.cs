// <copyright file="DomainCommandHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Aggregates;

using Microsoft.Extensions.Logging;

/// <summary>
/// Command handler for domain commands.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
public abstract partial class DomainCommandHandler<TCommand> : IDomainCommandHandler<TCommand>
{
    private readonly ILogger<DomainCommandHandler<TCommand>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DomainCommandHandler{TCommand}"/> class.
    /// </summary>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="logger">The logger.</param>
    protected DomainCommandHandler(TimeProvider timeProvider, ILogger<DomainCommandHandler<TCommand>> logger)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(logger);
        Time = timeProvider;
        _logger = logger;
    }

    /// <summary>
    /// Gets the time provider.
    /// </summary>
    protected TimeProvider Time { get; }

    /// <inheritdoc/>
    public abstract Task<ExecuteCommandResult> DoAsync(TCommand command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public async Task<ExecuteCommandResult> DoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        LogExecutingCommand(metadata.Message.Name, metadata.AggregateGlobalId, metadata.Message.Id, metadata.Context.CorrelationId, metadata.Context.UserId);
        ExecuteCommandResult result = await DoAsync(ToCommand(command), metadata, aggregate, cancellationToken);
        if (result.Failed)
        {
            LogCommandFailed(metadata.Message.Name, metadata.AggregateGlobalId, metadata.Message.Id, metadata.Context.CorrelationId, metadata.Context.UserId);
        }
        else
        {
            LogCommandExecutedSucceeded(metadata.Message.Name, metadata.AggregateGlobalId, metadata.Message.Id, metadata.Context.CorrelationId, metadata.Context.UserId);
        }

        return result;
    }

    /// <inheritdoc/>
    public virtual Task<ExecuteCommandResult> UndoAsync(TCommand command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
        => Task.FromException<ExecuteCommandResult>(new NotSupportedException());

    /// <inheritdoc/>
    public Task<ExecuteCommandResult> UndoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken) => UndoAsync(ToCommand(command), metadata, aggregate, cancellationToken);

    /// <summary>
    /// Checks if the aggregate is valid based on the provided metadata.
    /// </summary>
    /// <typeparam name="TAggregate">The type of the domain aggregate.</typeparam>
    /// <param name="aggregate">The domain aggregate to check.</param>
    /// <param name="metadata">The metadata containing the expected aggregate information.</param>
    /// <exception cref="CommandHandlerAggregateNullException">Thrown when the aggregate is null.</exception>
    /// <exception cref="CommandHandlerAggregateNameMismatchException">Thrown when the aggregate name does not match the expected name.</exception>
    /// <exception cref="CommandHandlerAggregateIdentifierMismatchException">Thrown when the aggregate identifier does not match the expected identifier.</exception>
    /// <returns>The validated domain aggregate.</returns>
    protected TAggregate CheckAggregateIsValid<TAggregate>(IDomainAggregate? aggregate, Metadata metadata)
        where TAggregate : IDomainAggregate
    {
        if (aggregate is null)
        {
            // The aggregate is null, this is an invalid state.
            LogAggregateNullError(
                metadata.Message.Name,
                metadata.AggregateGlobalId,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                metadata.Context.UserId);
            throw new CommandHandlerAggregateNullException(metadata);
        }

        if (aggregate.AggregateName != metadata.Message.Aggregate.Name)
        {
            LogAggregateNameMismatchError(
                aggregate.AggregateName,
                metadata.Message.Name,
                metadata.AggregateGlobalId,
                aggregate.AggregateName,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                metadata.Context.UserId);
            throw new CommandHandlerAggregateNameMismatchException(aggregate.AggregateName, metadata);
        }

        if (aggregate.AggregateId != metadata.Message.Aggregate.Id)
        {
            LogAggregateIdMismatchError(
                aggregate.AggregateId,
                metadata.Message.Name,
                metadata.AggregateGlobalId,
                aggregate.AggregateId,
                metadata.Message.Id,
                metadata.Context.CorrelationId,
                metadata.Context.UserId);
            throw new CommandHandlerAggregateIdentifierMismatchException(aggregate.AggregateId, metadata);
        }

        return (TAggregate)aggregate;
    }

    /// <summary>
    /// Converts to command.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <returns>TCommand.</returns>
    /// <exception cref="System.ArgumentException">command.</exception>
    private static TCommand ToCommand(object command)
    {
        return command is TCommand c
            ? c
            : throw new ArgumentException($"Invalid command type. Expected: {typeof(TCommand).Name}. Command: {JsonSerializer.Serialize(command)}", nameof(command));
    }

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "Aggregate ID mismatch for command '{CommandName}' on aggregate '{AggregateGlobalId}'. Expected='{ExpectedAggregateId}'; Actual='{ActualId}'; MessageId='{MessageId}'; CorrelationId='{CorrelationId}'; UserId='{UserId}'.")]
    private partial void LogAggregateIdMismatchError(string actualId, string commandName, string aggregateGlobalId, string expectedAggregateId, string messageId, string correlationId, string userId);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "Aggregate name mismatch for command {CommandName} on aggregate {AggregateGlobalId}. Expected='{ExpectedAggregateName}'; Actual='{ActualName}'; MessageId='{MessageId}'; CorrelationId='{CorrelationId}'; UserId='{UserId}'.")]
    private partial void LogAggregateNameMismatchError(string actualName, string commandName, string aggregateGlobalId, string expectedAggregateName, string messageId, string correlationId, string userId);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Aggregate is null for command {CommandName} on aggregate {AggregateGlobalId}. MessageId='{MessageId}'; CorrelationId='{CorrelationId}'; UserId='{UserId}'.")]
    private partial void LogAggregateNullError(string commandName, string aggregateGlobalId, string messageId, string correlationId, string userId);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Command {CommandName} on aggregate {AggregateGlobalId} succeeded. MessageId='{MessageId}'; CorrelationId='{CorrelationId}'; UserId='{UserId}'.")]
    private partial void LogCommandExecutedSucceeded(string commandName, string aggregateGlobalId, string messageId, string correlationId, string userId);

    [LoggerMessage(EventId = 1, Level = LogLevel.Warning, Message = "Command {CommandName} on aggregate {AggregateGlobalId} failed. MessageId='{MessageId}'; CorrelationId='{CorrelationId}'; UserId='{UserId}'.")]
    private partial void LogCommandFailed(string commandName, string aggregateGlobalId, string messageId, string correlationId, string userId);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Executing command {CommandName} on aggregate {AggregateGlobalId}. MessageId='{MessageId}'; CorrelationId='{CorrelationId}'; UserId='{UserId}'.")]
    private partial void LogExecutingCommand(string commandName, string aggregateGlobalId, string messageId, string correlationId, string userId);
}
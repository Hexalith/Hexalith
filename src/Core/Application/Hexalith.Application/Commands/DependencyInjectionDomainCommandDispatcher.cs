﻿// <copyright file="DependencyInjectionDomainCommandDispatcher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Command dispatcher that uses dependency injection to resolve command handlers.
/// </summary>
public partial class DependencyInjectionDomainCommandDispatcher : IDomainCommandDispatcher
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<DependencyInjectionDomainCommandDispatcher> _logger;

    /// <summary>
    /// The service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionDomainCommandDispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public DependencyInjectionDomainCommandDispatcher(IServiceProvider serviceProvider, ILogger<DependencyInjectionDomainCommandDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<ExecuteCommandResult> DoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        try
        {
            LogDispatchingCommandDebugInformation(metadata.Message.Name, metadata.Message.Aggregate.Name, metadata.Message.Aggregate.Id);
            return await GetHandler(command).DoAsync(command, metadata, aggregate, cancellationToken).ConfigureAwait(false);
        }
        catch (ApplicationErrorException ex)
        {
            LogDispatchingCommandErrorInformation(metadata.Message.Name, metadata.Message.Aggregate.Name, metadata.Message.Aggregate.Id);
            ex.Error?.LogApplicationErrorDetails(_logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            LogDispatchingCommandErrorInformation(
                ex,
                metadata.Message.Name,
                metadata.Message.Aggregate.Name,
                metadata.Message.Aggregate.Id,
                ex.FullMessage());
            throw;
        }
    }

    [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Dispatching command {CommandType} with aggregate id {AggregateName}-{AggregateId}")]
    public partial void LogDispatchingCommandDebugInformation(string CommandType, string AggregateName, string AggregateId);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Error while dispatching command {CommandType} with aggregate id {AggregateName}-{AggregateId} : {ErrorMessage}")]
    public partial void LogDispatchingCommandErrorInformation(Exception ex, string CommandType, string AggregateName, string AggregateId, string ErrorMessage);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Error while dispatching command {CommandType} with aggregate id {AggregateName}-{AggregateId}")]
    public partial void LogDispatchingCommandErrorInformation(string CommandType, string AggregateName, string AggregateId);

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Dispatching command {CommandType} undo with aggregate id {AggregateName}-{AggregateId}")]
    public partial void LogDispatchingCommandUndoDebugInformation(string CommandType, string AggregateName, string AggregateId);

    /// <inheritdoc/>
    public async Task<ExecuteCommandResult> UnDoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(aggregate);
        LogDispatchingCommandUndoDebugInformation(metadata.Message.Name, metadata.Message.Aggregate.Name, metadata.Message.Aggregate.Id);
        return await GetHandler(command).UndoAsync(command, metadata, aggregate, cancellationToken).ConfigureAwait(false);
    }

    private IDomainCommandHandler GetHandler(object command)
    {
        Type commandType = command.GetType();
        Type commandHandlerType = typeof(IDomainCommandHandler<>).MakeGenericType(commandType);
        return _serviceProvider.GetService(commandHandlerType) is not IDomainCommandHandler commandHandler
            ? throw new InvalidOperationException($"No handler found for command {commandType.Name}. Please add a IDomainCommandHandler<{commandType.Name}> to the service collection.")
            : commandHandler;
    }
}
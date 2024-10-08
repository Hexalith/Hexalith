﻿// <copyright file="DependencyInjectionCommandDispatcher.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Class DependencyInjectionCommandDispatcher.
/// Implements the <see cref="ICommandDispatcher" />.
/// </summary>
/// <seealso cref="ICommandDispatcher" />
[Obsolete("Use DependencyInjectionDomainCommandDispatcher instead", false)]
public partial class DependencyInjectionCommandDispatcher : ICommandDispatcher
{
    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<DependencyInjectionCommandDispatcher> _logger;

    /// <summary>
    /// The service provider.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionCommandDispatcher" /> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public DependencyInjectionCommandDispatcher(IServiceProvider serviceProvider, ILogger<DependencyInjectionCommandDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BaseMessage>> DoAsync(ICommand command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        try
        {
            LogDispatchingCommandDebugInformation(command.TypeName, command.AggregateName, command.AggregateId);
            IEnumerable<BaseMessage> events = await GetHandler(command).DoAsync(command, aggregate, cancellationToken).ConfigureAwait(false);

            return events;
        }
        catch (ApplicationErrorException ex)
        {
            LogDispatchingCommandErrorInformation(command.TypeName, command.AggregateName, command.AggregateId);
            ex.Error?.LogApplicationErrorDetails(_logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            LogDispatchingCommandErrorInformation(
                ex,
                command.TypeName,
                command.AggregateName,
                command.AggregateId,
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
    public async Task<IEnumerable<BaseMessage>> UnDoAsync(ICommand command, IAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        LogDispatchingCommandUndoDebugInformation(command.TypeName, command.AggregateName, command.AggregateId);
        IEnumerable<BaseMessage> events = await GetHandler(command).UndoAsync(command, aggregate, cancellationToken).ConfigureAwait(false);
        return events;
    }

    /// <summary>
    /// Gets the command handler of type ICommandHandler.<MyCommand>.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <returns>ICommandHandler<Command>.</returns>
    /// <exception cref="System.InvalidOperationException">No handler found for command {commandType.Name}. Please add a ICommandHandler.<{commandType.Name}> to the service collection.</exception>
    private ICommandHandler GetHandler(ICommand command)
    {
        Type commandType = command.GetType();
        Type commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        return _serviceProvider.GetService(commandHandlerType) is not ICommandHandler commandHandler
            ? throw new InvalidOperationException($"No handler found for command {commandType.Name}. Please add a ICommandHandler<{commandType.Name}> to the service collection.")
            : commandHandler;
    }
}
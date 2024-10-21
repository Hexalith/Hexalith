// <copyright file="DependencyInjectionDomainCommandDispatcher.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Aggregates;
using Hexalith.Extensions.Errors;
using Hexalith.Extensions.Helpers;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a command dispatcher that uses dependency injection to resolve command handlers.
/// This class implements the <see cref="IDomainCommandDispatcher"/> interface.
/// </summary>
/// <remarks>
/// This dispatcher uses the provided <see cref="IServiceProvider"/> to dynamically resolve
/// the appropriate command handler for each command type at runtime.
/// </remarks>
public partial class DependencyInjectionDomainCommandDispatcher : IDomainCommandDispatcher
{
    private readonly ILogger<DependencyInjectionDomainCommandDispatcher> _logger;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyInjectionDomainCommandDispatcher"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve command handlers.</param>
    /// <param name="logger">The logger used for logging operations within the dispatcher.</param>
    /// <exception cref="ArgumentNullException">Thrown if serviceProvider or logger is null.</exception>
    public DependencyInjectionDomainCommandDispatcher(IServiceProvider serviceProvider, ILogger<DependencyInjectionDomainCommandDispatcher> logger)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(logger);
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Executes the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="aggregate">The domain aggregate, if any.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the execution result of the command.</returns>
    /// <exception cref="ArgumentNullException">Thrown if command or metadata is null.</exception>
    /// <exception cref="ApplicationErrorException">Thrown if an application-specific error occurs during command execution.</exception>
    /// <remarks>
    /// This method logs debug information before executing the command and error information if an exception occurs.
    /// It uses the appropriate command handler resolved from the service provider.
    /// </remarks>
    public async Task<ExecuteCommandResult> DoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        try
        {
            LogDispatchingCommandDebugInformation(metadata.Message.Name, metadata.PartitionKey);
            return await GetHandler(command).DoAsync(command, metadata, aggregate, cancellationToken).ConfigureAwait(false);
        }
        catch (ApplicationErrorException ex)
        {
            LogDispatchingCommandErrorInformation(metadata.Message.Name, metadata.PartitionKey);
            ex.Error?.LogApplicationErrorDetails(_logger, ex);
            throw;
        }
        catch (Exception ex)
        {
            LogDispatchingCommandErrorInformation(
                ex,
                metadata.Message.Name,
                metadata.PartitionKey,
                ex.FullMessage());
            throw;
        }
    }

    /// <summary>
    /// Logs the debug information for the command being dispatched.
    /// </summary>
    /// <param name="commandType">The type of the command being dispatched.</param>
    /// <param name="aggregateKey">The key of the aggregate associated with the command.</param>
    [LoggerMessage(EventId = 0, Level = LogLevel.Debug, Message = "Dispatching command {CommandType} with aggregate key {AggregateKey}")]
    public partial void LogDispatchingCommandDebugInformation(string commandType, string aggregateKey);

    /// <summary>
    /// Logs the error information for the command being dispatched.
    /// </summary>
    /// <param name="ex">The exception that occurred during command dispatch.</param>
    /// <param name="commandType">The type of the command being dispatched.</param>
    /// <param name="aggregateKey">The key of the aggregate associated with the command.</param>
    /// <param name="errorMessage">The error message describing the exception.</param>
    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Error while dispatching command {CommandType} with aggregate id {AggregateKey} : {ErrorMessage}")]
    public partial void LogDispatchingCommandErrorInformation(Exception ex, string commandType, string aggregateKey, string errorMessage);

    /// <summary>
    /// Logs the error information for the command being dispatched.
    /// </summary>
    /// <param name="commandType">The type of the command being dispatched.</param>
    /// <param name="aggregateKey">The key of the aggregate associated with the command.</param>
    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "Error while dispatching command {CommandType} with aggregate id {AggregateKey}")]
    public partial void LogDispatchingCommandErrorInformation(string commandType, string aggregateKey);

    /// <summary>
    /// Logs the debug information for the command being undone.
    /// </summary>
    /// <param name="commandType">The type of the command being undone.</param>
    /// <param name="aggregateKey">The key of the aggregate associated with the command.</param>
    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Dispatching command {CommandType} undo with aggregate id {AggregateKey}")]
    public partial void LogDispatchingCommandUndoDebugInformation(string commandType, string aggregateKey);

    /// <summary>
    /// Undoes the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to undo.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the execution result of the undo operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if command, metadata, or aggregate is null.</exception>
    /// <remarks>
    /// This method logs debug information before undoing the command.
    /// It uses the appropriate command handler resolved from the service provider to perform the undo operation.
    /// </remarks>
    public async Task<ExecuteCommandResult> UnDoAsync(object command, Metadata metadata, IDomainAggregate? aggregate, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        ArgumentNullException.ThrowIfNull(aggregate);
        LogDispatchingCommandUndoDebugInformation(metadata.Message.Name, metadata.PartitionKey);
        return await GetHandler(command).UndoAsync(command, metadata, aggregate, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the appropriate command handler for the specified command.
    /// </summary>
    /// <param name="command">The command for which to get the handler.</param>
    /// <returns>An instance of <see cref="IDomainCommandHandler"/> that can handle the specified command.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no suitable handler is found for the command.</exception>
    /// <remarks>
    /// This method uses the service provider to resolve the appropriate command handler based on the command type.
    /// It constructs the generic type for the command handler and attempts to resolve it from the service provider.
    /// </remarks>
    private IDomainCommandHandler GetHandler(object command)
    {
        Type commandType = command.GetType();
        Type commandHandlerType = typeof(IDomainCommandHandler<>).MakeGenericType(commandType);
        return _serviceProvider.GetService(commandHandlerType) is not IDomainCommandHandler commandHandler
            ? throw new InvalidOperationException($"No handler found for command {commandType.Name}. Please add a IDomainCommandHandler<{commandType.Name}> to the service collection.")
            : commandHandler;
    }
}
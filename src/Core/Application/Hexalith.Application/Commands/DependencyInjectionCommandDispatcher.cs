// <copyright file="DependencyInjectionCommandDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Domain.Abstractions.Events;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class DependencyInjectionCommandDispatcher.
/// Implements the <see cref="ICommandDispatcher" />.
/// </summary>
/// <seealso cref="ICommandDispatcher" />
public class DependencyInjectionCommandDispatcher : ICommandDispatcher
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
        _serviceProvider = Guard.Against.Null(serviceProvider);
        _logger = Guard.Against.Null(logger);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BaseEvent>> DoAsync(ICommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Dispatching command {CommandType} with aggregate id {AggregateName}-{AggregateId}", command.MessageName, command.AggregateName, command.AggregateId);
        IEnumerable<BaseEvent> events = await GetHandler(command).DoAsync(command, cancellationToken);
        return events;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BaseEvent>> UnDoAsync(ICommand command, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Dispatching command {CommandType} undo with aggregate id {AggregateName}-{AggregateId}", command.MessageName, command.AggregateName, command.AggregateId);
        IEnumerable<BaseEvent> events = await GetHandler(command).UndoAsync(command, cancellationToken);
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

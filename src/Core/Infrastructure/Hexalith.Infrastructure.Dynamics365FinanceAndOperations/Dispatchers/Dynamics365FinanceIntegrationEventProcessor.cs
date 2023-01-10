// <copyright file="Dynamics365FinanceIntegrationEventProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Dispatchers;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Errors;
using Hexalith.Application.Abstractions.Events;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Application.Events;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class Dynamics365FinanceIntegrationEventDispatcher.
/// Implements the <see cref="Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Dispatchers.IDynamics365FinanceIntegrationEventProcessor" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Dispatchers.IDynamics365FinanceIntegrationEventProcessor" />
public class Dynamics365FinanceIntegrationEventProcessor : DependencyInjectionEventDispatcher, IDynamics365FinanceIntegrationEventProcessor
{
    /// <summary>
    /// The command processor.
    /// </summary>
    private readonly ICommandProcessor _commandProcessor;

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceIntegrationEventProcessor" /> class.
    /// </summary>
    /// <param name="commandProcessor">The command processor.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public Dynamics365FinanceIntegrationEventProcessor(
        ICommandProcessor commandProcessor,
        IDateTimeService dateTimeService,
        IServiceProvider serviceProvider,
        ILogger<Dynamics365FinanceIntegrationEventProcessor> logger)
        : base(serviceProvider, logger)
    {
        _commandProcessor = Guard.Against.Null(commandProcessor);
        _dateTimeService = Guard.Against.Null(dateTimeService);
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(Dynamics365BusinessEventBase @event, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(@event);
        try
        {
            IEnumerable<BaseCommand> commands = (await ApplyAsync(@event, cancellationToken)).SelectMany(p => p);
            if (!commands.Any())
            {
                commands = @event.ToCommand().IntoArray();
            }

            foreach (BaseCommand command in commands)
            {
                await _commandProcessor.SubmitAsync(
                    command,
                    Metadata.CreateNew(command, @event, _dateTimeService.UtcNow),
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationErrorException(new EventDispatchFailed(@event, ex));
        }
    }

    /// <inheritdoc/>
    public Task SubmitAsync(IEvent @event, CancellationToken cancellationToken)
    {
        return @event is Dynamics365BusinessEventBase businessEvent
            ? SubmitAsync(businessEvent, cancellationToken)
            : throw new ApplicationErrorException(new EventNotSupportedByDispatcher(nameof(Dynamics365FinanceIntegrationEventProcessor)));
    }
}

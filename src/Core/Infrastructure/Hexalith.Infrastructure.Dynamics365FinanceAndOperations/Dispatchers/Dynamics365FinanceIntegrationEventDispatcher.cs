// <copyright file="Dynamics365FinanceIntegrationEventDispatcher.cs" company="Fiveforty SAS Paris France">
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
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using System.Threading;
using System.Threading.Tasks;

public class Dynamics365FinanceIntegrationEventDispatcher : IDynamics365FinanceIntegrationEventDispatcher
{
    private readonly ICommandProcessor _commandProcessor;
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceIntegrationEventDispatcher"/> class.
    /// </summary>
    /// <param name="commandProcessor"></param>
    public Dynamics365FinanceIntegrationEventDispatcher(ICommandProcessor commandProcessor, IDateTimeService dateTimeService)
    {
        _commandProcessor = Guard.Against.Null(commandProcessor);
        _dateTimeService = Guard.Against.Null(dateTimeService);
    }

    /// <inheritdoc/>
    public async Task DispatchAsync(Dynamics365BusinessEventBase @event, CancellationToken cancellationToken)
    {
        _ = Guard.Against.Null(@event);
        try
        {
            BaseCommand command = @event.ToCommand();
            await _commandProcessor.SubmitAsync(
                command,
                Metadata.CreateNew(command, @event, _dateTimeService.UtcNow),
                cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationErrorException(new EventDispatchFailed(@event, ex));
        }
    }

    /// <inheritdoc/>
    public Task DispatchAsync(object @event, CancellationToken cancellationToken)
    {
        return @event is Dynamics365BusinessEventBase businessEvent
            ? DispatchAsync(businessEvent, cancellationToken)
            : throw new ApplicationErrorException(new EventNotSupportedByDispatcher(nameof(Dynamics365FinanceIntegrationEventDispatcher)));
    }

    /// <inheritdoc/>
    public Task DispatchAsync(IEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

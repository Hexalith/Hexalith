namespace Hexalith.Infrastructure.WebServer.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Queries;
    using Hexalith.Domain.ValueTypes;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public abstract class QueryCommandControllerBase : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IQueryBus _queryDispatcher;

        protected QueryCommandControllerBase(IQueryBus queryDispatcher, ILogger logger)
        {
            _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<IActionResult> Ask<TQuery>(TQuery query, string? messageId = null)
            where TQuery : class
        {
            MessageId msgId = string.IsNullOrWhiteSpace(messageId) ? new MessageId() : new MessageId(messageId);
            if (string.IsNullOrWhiteSpace(User?.Identity?.Name))
                return Unauthorized("User name is not defined");
            _logger.LogDebug($"User '{User.Identity.Name}' asked for query : {typeof(TQuery).Name}");
            try
            {
                return Ok(await _queryDispatcher
                    .Dispatch(new Envelope<TQuery>(
                        query,
                        msgId,
                        User.Identity.Name,
                        DateTimeOffset.Now
                        ))
                    .ConfigureAwait(false));
            }
            catch (QueryHandlerNotFoundException e)
            {
                _logger.LogError($"Error while asking for query '{typeof(TQuery).Name}' by the user '{User.Identity.Name}'.\n{e.Message}");
                return BadRequest(new { Query = typeof(TQuery).Name });
            }
            catch (BusinessObjectNotFoundException ex)
            {
                _logger.LogError($"User '{User.Identity.Name}' asked for a not found business object '{ex.Name}' with id '{ex.Id}'. Query {typeof(TQuery).Name}");
                return NotFound(new { ex.Id, ex.Name });
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while asking for query '{typeof(TQuery).Name}' by the user '{User.Identity.Name}'.\n{e.Message}");
                return StatusCode(500);
            }
        }

        protected async Task<IActionResult> Tell<TCommand>(TCommand command, string? messageId = null)
            where TCommand : class
        {
            MessageId msgId = string.IsNullOrWhiteSpace(messageId) ? new MessageId() : new MessageId(messageId);

            if (string.IsNullOrWhiteSpace(User?.Identity?.Name))
                return Unauthorized("User name is not defined");
            _logger.LogDebug($"User '{User.Identity.Name}' told to execute command : {typeof(TCommand).Name}");
            try
            {
                return Ok(await _queryDispatcher
                    .Dispatch(new Envelope<TCommand>(
                        command,
                        msgId,
                        User.Identity.Name,
                        DateTimeOffset.Now
                        ))
                    .ConfigureAwait(false));
            }
            catch (CommandHandlerNotFoundException)
            {
                _logger.LogError($"User '{User.Identity.Name}' told to execute an unkown command : {typeof(TCommand).Name}");
                return BadRequest(new { Query = typeof(TCommand).Name });
            }
            catch (BusinessObjectNotFoundException ex)
            {
                _logger.LogError($"User '{User.Identity.Name}' tried to execute an action on business object '{ex.Name}' with id '{ex.Id}', but it's not found. Command {typeof(TCommand).Name}");
                return NotFound(new { ex.Id, ex.Name });
            }
        }
    }
}
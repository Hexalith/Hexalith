// <copyright file="ClientRequestService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;
using Hexalith.Application.Requests;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.Application.States;
using Hexalith.Extensions.Helpers;
using Hexalith.PolymorphicSerializations;

using Microsoft.Extensions.Logging;
using Hexalith.Commons.UniqueIds;
using Hexalith.Applications.States;

/// <summary>
/// Represents a service for sending requests asynchronously.
/// </summary>
public partial class ClientRequestService : IRequestService
{
    private readonly HttpClient _client;
    private readonly ILogger<ClientRequestService> _logger;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClientRequestService"/> class.
    /// </summary>
    /// <param name="client">The HTTP client.</param>
    /// <param name="sessionService">The user session service.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="logger">The logger.</param>
    public ClientRequestService(
        [NotNull] HttpClient client,
        [NotNull] ISessionService sessionService,
        [NotNull] TimeProvider timeProvider,
        [NotNull] ILogger<ClientRequestService> logger)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(logger);
        _client = client;
        _sessionService = sessionService;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TRequest> SubmitAsync<TRequest>(ClaimsPrincipal user, TRequest request, CancellationToken cancellationToken)
        where TRequest : Polymorphic
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(user);

        string? userName = user.Identity?.Name;
        if (string.IsNullOrWhiteSpace(userName))
        {
            throw new InvalidOperationException("User is authenticated but user ID is not found.");
        }

        string messageId = UniqueIdHelper.GenerateUniqueStringId();
        SessionInformation session = await _sessionService.GetAsync(userName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Session not found or expired.");
        Metadata metadata = new(
            new MessageMetadata(
            messageId,
            string.Empty,
            1,
            request.CreateDomainMetadata(),
            _timeProvider.GetLocalNow()),
            new ContextMetadata(
                messageId,
                userName,
                session.PartitionId,
                _timeProvider.GetLocalNow(),
                null,
                null,
                null,
                session.SessionId,
                []));

        return await SubmitRequestAsync(request, metadata, cancellationToken).ConfigureAwait(false);
    }

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Request '{RequestName}' succeeded on aggregate '{AggregateId}' for user '{UserId}'.")]
    private static partial void LogRequestSucceeded(ILogger<ClientRequestService> logger, string requestName, string aggregateId, string userId);

    /// <summary>
    /// Submits a request asynchronously with the provided metadata.
    /// </summary>
    /// <param name="request">The request to submit.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    [SuppressMessage("Critical Code Smell", "S2302:\"nameof\" should be used", Justification = "NA")]
    private async Task<TRequest> SubmitRequestAsync<TRequest>(TRequest request, Metadata metadata, CancellationToken cancellationToken)
    where TRequest : Polymorphic
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);
        if (request is not Polymorphic recordBase)
        {
            throw new ArgumentException($"The request should be of type {nameof(Polymorphic)}");
        }

        HttpResponseMessage response = await _client.PostAsJsonAsync("api/request/submit", new MessageState(recordBase, metadata), cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Request {metadata.Message.Name} failed on aggregate '{metadata.DomainGlobalId}'. The response status code was {response.StatusCode}.");
        }

        MessageState? messageState = await response
            .Content
            .ReadFromJsonAsync<MessageState>(cancellationToken: cancellationToken).ConfigureAwait(false);
        if (messageState is null)
        {
            string value = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            throw new InvalidOperationException($"Request {metadata.Message.Name} failed on aggregate '{metadata.DomainGlobalId}'. Failed to deserialize response : " + value);
        }

        if (messageState.MessageObject is not TRequest result)
        {
            throw new InvalidOperationException(
                $"Request {metadata.Message.Name} failed on aggregate '{metadata.DomainGlobalId}'." +
                $"Expected response of type {typeof(TRequest).Name} but received {messageState.MessageObject.GetType().Name}.");
        }

        LogRequestSucceeded(
            _logger,
            metadata.Message.Name,
            metadata.DomainGlobalId,
            metadata.Context.UserId);

        return result;
    }
}
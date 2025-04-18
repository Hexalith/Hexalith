// <copyright file="ApiServerRequestService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Requests;
using Hexalith.Application.Sessions.Models;
using Hexalith.Application.Sessions.Services;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a service for sending requests asynchronously.
/// </summary>
public class ApiServerRequestService : IRequestService
{
    private readonly IRequestProcessor _requestProcessor;
    private readonly ISessionService _sessionService;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiServerRequestService"/> class.
    /// </summary>
    /// <param name="requestProcessor">The request processor.</param>
    /// <param name="timeProvider">The time provider.</param>
    /// <param name="sessionService">The session service.</param>
    public ApiServerRequestService(
        [NotNull] IRequestProcessor requestProcessor,
        [NotNull] TimeProvider timeProvider,
        [NotNull] ISessionService sessionService)
    {
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(sessionService);
        ArgumentNullException.ThrowIfNull(requestProcessor);
        _requestProcessor = requestProcessor;
        _timeProvider = timeProvider;
        _sessionService = sessionService;
    }

    /// <inheritdoc/>
    public async Task<TRequest> SubmitAsync<TRequest>(ClaimsPrincipal user, TRequest request, CancellationToken cancellationToken)
        where TRequest : Polymorphic
    {
        ArgumentNullException.ThrowIfNull(request);
        if (string.IsNullOrWhiteSpace(user.Identity?.Name))
        {
            throw new InvalidOperationException("User name empty.");
        }

        SessionInformation session = await _sessionService.GetAsync(user.Identity.Name, cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(session.PartitionId))
        {
            throw new InvalidOperationException("Partition not set. PartitionId is empty.");
        }

        Metadata metadata = Metadata.CreateNew(request, user.Identity.Name, session.PartitionId, _timeProvider.GetLocalNow());

        return await _requestProcessor
            .ProcessAsync(request, metadata, cancellationToken)
            .ConfigureAwait(false) as TRequest
                ?? throw new InvalidOperationException(
                    $"Request processor returned a null or invalid object while processing the '{metadata.Message.Name}' request.");
    }
}
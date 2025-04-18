// <copyright file="ServiceApiProxy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Proxies.Services;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Sessions.Services;
using Hexalith.Application.States;
using Hexalith.PolymorphicSerializations;

using Microsoft.Extensions.Logging;

/// <summary>
/// Http API service proxy base class.
/// </summary>
public partial class ServiceApiProxy
{
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceApiProxy"/> class.
    /// </summary>
    /// <param name="userPartitionService">The <see cref="IUserPartitionService"/> instance used for user partition operations.</param>
    /// <param name="httpClient">The <see cref="HttpClient"/> instance used for making HTTP requests.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/> instance used for time operations.</param>
    /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
    public ServiceApiProxy(
        IUserPartitionService userPartitionService,
        HttpClient httpClient,
        TimeProvider timeProvider,
        ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(userPartitionService);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(timeProvider);
        ArgumentNullException.ThrowIfNull(logger);
        UserPartitionService = userPartitionService;
        HttpClient = httpClient;
        _timeProvider = timeProvider;
        Logger = logger;
    }

    /// <summary>
    /// Gets the <see cref="HttpClient"/> instance used for making HTTP requests.
    /// </summary>
    protected HttpClient HttpClient { get; }

    /// <summary>
    /// Gets the <see cref="ILogger"/> instance used for logging.
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the <see cref="IUserPartitionService"/> instance used for user partition operations.
    /// </summary>
    protected IUserPartitionService UserPartitionService { get; }

    /// <summary>
    /// Sends a POST request with the specified value to the specified route and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TValue">The type of the value to send.</typeparam>
    /// <param name="route">The route to send the request to.</param>
    /// <param name="value">The value to send in the request body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the POST request.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the route is null or whitespace.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the service returns an empty response.</exception>
    protected async Task<TResult> PostAsync<TResult, TValue>([NotNull] string route, [NotNull] TValue value, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(route);
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync(route, value, cancellationToken)
            .ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
        TResult? answer = await response.Content.ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        if (answer is null)
        {
            string valueJson = JsonSerializer.Serialize(value);
            string message = "The service returned an empty response for request : " + JsonSerializer.Serialize(value);
            LogEmptyResponse(Logger, route, valueJson);
            throw new InvalidOperationException(message);
        }

        return answer;
    }

    /// <summary>
    /// Sends a POST request with the specified message state to the specified route and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="route">The route to send the request to.</param>
    /// <param name="value">The message state to send in the request body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the POST request.</returns>
    protected async Task<TResult> PostAsync<TResult>([NotNull] string route, [NotNull] MessageState value, CancellationToken cancellationToken)
        => await PostAsync<TResult, MessageState>(route, value, cancellationToken);

    /// <summary>
    /// Sends a POST request with the specified value and metadata to the specified route and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TValue">The type of the value to send.</typeparam>
    /// <param name="route">The route to send the request to.</param>
    /// <param name="value">The value to send in the request body.</param>
    /// <param name="metadata">The metadata associated with the value.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the POST request.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the value or metadata is null.</exception>
    protected async Task<TResult> PostAsync<TResult, TValue>([NotNull] string route, [NotNull] TValue value, [NotNull] Metadata metadata, CancellationToken cancellationToken)
        where TValue : Polymorphic
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(metadata);
        return await PostAsync<TResult>(route, new MessageState(value, metadata), cancellationToken);
    }

    /// <summary>
    /// Sends a POST request with the specified value and user information to the specified route and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <typeparam name="TValue">The type of the value to send.</typeparam>
    /// <param name="route">The route to send the request to.</param>
    /// <param name="user">The user information associated with the request.</param>
    /// <param name="value">The value to send in the request body.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the POST request.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the user or user identity is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the user identity name is null or whitespace.</exception>
    protected async Task<TResult> PostAsync<TResult, TValue>([NotNull] string route, [NotNull] ClaimsPrincipal user, [NotNull] TValue value, CancellationToken cancellationToken)
        where TValue : Polymorphic
    {
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(user.Identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(user.Identity.Name);
        string userName = user.Identity.Name;
        string partitionId = await UserPartitionService.GetDefaultPartitionAsync(userName, cancellationToken);
        Metadata metadata = Metadata.CreateNew(value, userName, partitionId, _timeProvider.GetLocalNow());
        return await PostAsync<TResult>(route, new MessageState(value, metadata), cancellationToken);
    }

    /// <summary>
    /// Logs an empty response from the service.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="route">The route that was called.</param>
    /// <param name="value">The value that was sent in the request.</param>
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "The service call to '{Route}' returned an empty response for value : {value}")]
    private static partial void LogEmptyResponse(ILogger logger, string route, string value);
}
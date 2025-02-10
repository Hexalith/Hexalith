// <copyright file="RequestHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Provides helper methods for handling requests.
/// </summary>
public static class RequestHelper
{
    /// <summary>
    /// Finds the summary asynchronously.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="requestService">The request service.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="user">The user claims principal.</param>
    /// <param name="createRequest">The function to create the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the view model.</returns>
    /// <exception cref="ArgumentNullException">Thrown when requestService, user, or createRequest is null.</exception>
    public static async Task<TViewModel?> FindDetailsAsync<TViewModel, TRequest>(
        [NotNull] this IRequestService requestService,
        string? id,
        [NotNull] ClaimsPrincipal user,
        Func<string, TRequest> createRequest,
        CancellationToken cancellationToken)
        where TViewModel : class
        where TRequest : PolymorphicRecordBase, IRequest
    {
        ArgumentNullException.ThrowIfNull(requestService);
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(createRequest);
        TViewModel? model = null;
        if (!string.IsNullOrWhiteSpace(id))
        {
            model = (await requestService
                .SubmitAsync(user, createRequest(id), cancellationToken)
                .ConfigureAwait(false))?.Result as TViewModel;
        }

        return model;
    }

    /// <summary>
    /// Finds the summary asynchronously.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="requestService">The request service.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="user">The user.</param>
    /// <param name="createRequest">The function to create the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the view model.</returns>
    /// <exception cref="ArgumentNullException">Thrown when requestService, user, or createRequest is null.</exception>
    public static async Task<TViewModel?> FindSummaryAsync<TViewModel, TRequest>(
        [NotNull] this IRequestService requestService,
        string? id,
        [NotNull] ClaimsPrincipal user,
        Func<string, TRequest> createRequest,
        CancellationToken cancellationToken)
        where TViewModel : class
        where TRequest : PolymorphicRecordBase, IChunkableRequest
    {
        ArgumentNullException.ThrowIfNull(requestService);
        ArgumentNullException.ThrowIfNull(user);
        ArgumentNullException.ThrowIfNull(createRequest);
        TViewModel? model = null;
        if (!string.IsNullOrWhiteSpace(id))
        {
            model = (await requestService
                .SubmitAsync(user, createRequest(id), cancellationToken)
                .ConfigureAwait(false))?.Results?.FirstOrDefault() as TViewModel;
        }

        return model;
    }

    /// <summary>
    /// Gets the summary asynchronously.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="requestService">The request service.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="user">The user claims principal.</param>
    /// <param name="createRequest">The function to create the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the view model.</returns>
    /// <exception cref="ArgumentException">Thrown when id is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the request fails or the id is not found.</exception>
    public static async Task<TViewModel> GetDetailsAsync<TViewModel, TRequest>(
        [NotNull] this IRequestService requestService,
        [NotNull] string id,
        [NotNull] ClaimsPrincipal user,
        Func<string, TRequest> createRequest,
        CancellationToken cancellationToken)
        where TViewModel : class
        where TRequest : PolymorphicRecordBase, IRequest
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await RequestHelper.FindDetailsAsync<TViewModel, TRequest>(requestService, id, user, createRequest, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"The request {typeof(TRequest).Name} failed. '{id}' not found.");
    }

    /// <summary>
    /// Gets the summary asynchronously.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="requestService">The request service.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="user">The user claims principal.</param>
    /// <param name="createRequest">The function to create the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the view model.</returns>
    /// <exception cref="ArgumentException">Thrown when id is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the request fails or the id is not found.</exception>
    public static async Task<TViewModel> GetSummaryAsync<TViewModel, TRequest>(
        [NotNull] this IRequestService requestService,
        [NotNull] string id,
        [NotNull] ClaimsPrincipal user,
        Func<string, TRequest> createRequest,
        CancellationToken cancellationToken)
        where TViewModel : class
        where TRequest : PolymorphicRecordBase, IChunkableRequest
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await RequestHelper.FindSummaryAsync<TViewModel, TRequest>(requestService, id, user, createRequest, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"The request {typeof(TRequest).Name} failed. '{id}' not found.");
    }
}
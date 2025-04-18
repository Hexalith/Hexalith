// <copyright file="IRequestService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Security.Claims;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a service for submitting requests.
/// </summary>
/// <remarks>
/// This service is responsible for submitting requests and processing them asynchronously.
/// </remarks>
public interface IRequestService
{
    /// <summary>
    /// Submits a request asynchronously.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <param name="user">The user submitting the request.</param>
    /// <param name="request">The request to be submitted.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the submitted request.</returns>
    Task<TRequest> SubmitAsync<TRequest>(ClaimsPrincipal user, TRequest request, CancellationToken cancellationToken)
        where TRequest : Polymorphic;
}
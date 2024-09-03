// <copyright file="IRequestService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using Hexalith.Application.States;

/// <summary>
/// Represents a service for submitting requests.
/// </summary>
/// <remarks>
/// This service is responsible for submitting requests and processing them asynchronously.
/// </remarks>
/// <param name="request">The request to be submitted.</param>
/// <param name="cancellationToken">The cancellation token.</param>
public interface IRequestService
{
    /// <summary>
    /// Submits a request asynchronously.
    /// </summary>
    /// <param name="request">The request to be submitted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Task.</returns>
    Task SubmitRequestAsync(RequestState request, CancellationToken cancellationToken);
}
// <copyright file="IRequestProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;

/// <summary>
/// Defines a processor for handling requests.
/// </summary>
public interface IRequestProcessor
{
    /// <summary>
    /// Processes the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="metadata">The metadata associated with the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the processed request.</returns>
    Task<object> ProcessAsync(object request, Metadata metadata, CancellationToken cancellationToken);
}
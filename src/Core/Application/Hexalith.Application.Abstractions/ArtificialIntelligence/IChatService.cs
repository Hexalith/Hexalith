// <copyright file="IChatService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.ArtificialIntelligence;

/// <summary>
/// Interface IChatService.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Chats the asynchronous.
    /// </summary>
    /// <param name="ask">The ask.</param>
    /// <param name="openApiAuthentication">The open API authentication.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;Answer&gt;.</returns>
    Task<Answer> ChatAsync(Ask ask, OpenApiAuthentication openApiAuthentication, CancellationToken cancellationToken);
}
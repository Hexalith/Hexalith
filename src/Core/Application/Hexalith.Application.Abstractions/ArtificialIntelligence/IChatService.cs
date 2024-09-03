// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="IChatService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
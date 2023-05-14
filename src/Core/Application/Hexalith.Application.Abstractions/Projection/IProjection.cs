// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-01-2023
// ***********************************************************************
// <copyright file="IProjection.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Projection;

using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;

/// <summary>
/// Interface IProjection.
/// </summary>
public interface IProjection
{
    /// <summary>
    /// Executes the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.Object&gt;.</returns>
    Task<BaseNotification> ExecuteAsync(BaseRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Handles the specified domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task HandleAsync(BaseEvent domainEvent, CancellationToken cancellationToken);

    /// <summary>
    /// Submits the asynchronous.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="userId">The user identifier.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SubmitAsync(BaseRequest request, string correlationId, string userId, string sessionId, CancellationToken cancellationToken);
}
// <copyright file="IIntegrationEventProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Events;

using System.Threading.Tasks;

using Hexalith.Commons.Metadatas;

/// <summary>
/// The integration event handler interface.
/// </summary>
public interface IIntegrationEventProcessor
{
    /// <summary>
    /// Submits the asynchronous.
    /// </summary>
    /// <param name="baseEvent">The base event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task SubmitAsync(object baseEvent, Metadata metadata, CancellationToken cancellationToken);
}
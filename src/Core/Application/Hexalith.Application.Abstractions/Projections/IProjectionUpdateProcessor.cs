// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="IProjectionUpdateProcessor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Projections;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;

/// <summary>
/// Interface IProjectionUpdateProcessor.
/// </summary>
public interface IProjectionUpdateProcessor
{
    /// <summary>
    /// Applies the event asynchronously.
    /// </summary>
    /// <param name="ev">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task ApplyAsync(IEvent ev, IMetadata metadata, CancellationToken cancellationToken);
}
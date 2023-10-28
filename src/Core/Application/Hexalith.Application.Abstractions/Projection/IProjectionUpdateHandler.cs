// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="IProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// ***********************************************************************
namespace Hexalith.Application.Projection;

using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;

/// <summary>
/// Interface IProjectionUpdateHandler.
/// </summary>
public interface IProjectionUpdateHandler
{
    /// <summary>
    /// Applies the specified event.
    /// </summary>
    /// <param name="ev">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    public Task ApplyAsync(IEvent ev, IMetadata metadata, CancellationToken cancellationToken);
}
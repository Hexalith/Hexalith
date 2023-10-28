// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-27-2023
// ***********************************************************************
// <copyright file="ProjectionUpdateHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Projection;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Events;

/// <summary>
/// Class ProjectionUpdateHandler.
/// Implements the <see cref="Hexalith.Application.Projection.IProjectionUpdateHandler{TEvent}" />.
/// </summary>
/// <typeparam name="TEvent">The type of the t event.</typeparam>
/// <seealso cref="Hexalith.Application.Projection.IProjectionUpdateHandler{TEvent}" />
public abstract class ProjectionUpdateHandler<TEvent> : IProjectionUpdateHandler<TEvent>
    where TEvent : BaseEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectionUpdateHandler{TEvent}"/> class.
    /// </summary>
    /// <param name="stateStore">The state store.</param>
    protected ProjectionUpdateHandler([NotNull] IStateStoreProvider stateStore)
    {
        ArgumentNullException.ThrowIfNull(stateStore);
        StateStore = stateStore;
    }

    /// <summary>
    /// Gets the state store.
    /// </summary>
    /// <value>The state store.</value>
    protected IStateStoreProvider StateStore { get; }

    /// <summary>
    /// Applies the specified event.
    /// </summary>
    /// <param name="baseEvent">The event.</param>
    /// <param name="metadata">The metadata.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotImplementedException">null.</exception>
    public abstract Task ApplyAsync(TEvent baseEvent, IMetadata metadata, CancellationToken cancellationToken);
}
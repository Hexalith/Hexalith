// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="ActorProjectionFactory{TState}.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Projections;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Infrastructure.DaprRuntime.Actors;

/// <summary>
/// Class ActorProjectionFactory.
/// Implements the <see cref="Hexalith.Infrastructure.DaprRuntime.Projections.IActorProjectionFactory" />.
/// </summary>
/// <typeparam name="TState">The type of the t state.</typeparam>
/// <seealso cref="Hexalith.Infrastructure.DaprRuntime.Projections.IActorProjectionFactory" />
public abstract class ActorProjectionFactory<TState> : IActorProjectionFactory
    where TState : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActorProjectionFactory{TState}" /> class.
    /// </summary>
    /// <param name="actorFactory">The actor factory.</param>
    /// <param name="appName">Name of the application.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    protected ActorProjectionFactory(IActorProxyFactory actorFactory, string appName)
    {
        ArgumentNullException.ThrowIfNull(actorFactory);
        ArgumentException.ThrowIfNullOrEmpty(appName);
        ActorFactory = actorFactory;
        AppName = appName;
    }

    /// <summary>
    /// Gets the actor factory.
    /// </summary>
    /// <value>The actor factory.</value>
    protected IActorProxyFactory ActorFactory { get; }

    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    /// <value>The name of the application.</value>
    protected string AppName { get; }

    /// <inheritdoc/>
    public Task<T?> GetAsync<T>(string aggregateId, CancellationToken cancellationToken)
        where T : class
    {
        return Task.FromResult(typeof(T) != typeof(TState)
            ? throw new ArgumentException($"Type {typeof(T).Name} is not supported by projection factory '{GetType().Name}'. Expected : {typeof(TState).Name}.")
            : GetAsync(aggregateId, cancellationToken) as T);
    }

    /// <summary>
    /// Get as an asynchronous operation.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;TState&gt; representing the asynchronous operation.</returns>
    public async Task<TState?> GetAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        string? result = await GetProjectionActor(aggregateId)
            .GetAsync()
            .ConfigureAwait(false);
        return result is null ? null : JsonSerializer.Deserialize<TState>(result);
    }

    /// <inheritdoc/>
    public abstract IKeyValueActor GetProjectionActor(string aggregateId);

    /// <inheritdoc/>
    public async Task SetAsync<T>(string aggregateId, T state, CancellationToken cancellationToken)
        where T : class
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        await GetProjectionActor(aggregateId)
            .SetAsync(JsonSerializer.Serialize(state))
            .ConfigureAwait(false);
    }
}
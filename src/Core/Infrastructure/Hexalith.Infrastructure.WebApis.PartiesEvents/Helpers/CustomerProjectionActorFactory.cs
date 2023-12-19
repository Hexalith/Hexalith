// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis.Parties
// Author           : Jérôme Piquot
// Created          : 10-27-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="CustomerProjectionActorFactory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers;

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dapr.Actors.Client;

using Hexalith.Domain.Events;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Parties.Helpers;

/// <summary>
/// Class CustomerProjectionActorFactory.
/// Implements the <see cref="Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers.ICustomerProjectionActorFactory" />.
/// </summary>
/// <seealso cref="Hexalith.Infrastructure.WebApis.PartiesEvents.Helpers.ICustomerProjectionActorFactory" />
public class CustomerProjectionActorFactory : ICustomerProjectionActorFactory
{
    /// <summary>
    /// The actor factory.
    /// </summary>
    private readonly IActorProxyFactory _actorFactory;

    /// <summary>
    /// The application name.
    /// </summary>
    private readonly string _appName;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerProjectionActorFactory"/> class.
    /// </summary>
    /// <param name="actorFactory">The actor factory.</param>
    /// <param name="appName">Name of the application.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public CustomerProjectionActorFactory(IActorProxyFactory actorFactory, string appName)
    {
        ArgumentNullException.ThrowIfNull(actorFactory);
        ArgumentException.ThrowIfNullOrEmpty(appName);
        _actorFactory = actorFactory;
        _appName = appName;
    }

    /// <inheritdoc/>
    public async Task<T?> GetAsync<T>(string aggregateId, CancellationToken cancellationToken)
        where T : class
    {
        ArgumentNullException.ThrowIfNullOrEmpty(aggregateId);
        if (typeof(T) != typeof(CustomerRegistered))
        {
            throw new ArgumentException($"Type {typeof(T).Name} is not supported by {nameof(CustomerProjectionActorFactory)}. Expected : {nameof(CustomerRegistered)}.");
        }

        CustomerRegistered? state = await GetAsync(aggregateId, cancellationToken).ConfigureAwait(false);
        return state as T;
    }

    /// <inheritdoc/>
    public async Task<CustomerRegistered?> GetAsync(string aggregateId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(aggregateId);
        string? result = await GetProjectionActor(aggregateId).GetAsync().ConfigureAwait(false);
        return result is null ? null : JsonSerializer.Deserialize<CustomerRegistered>(result);
    }

    /// <inheritdoc/>
    public IKeyValueActor GetProjectionActor(string aggregateId)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(aggregateId);
        return _actorFactory.GetCustomerProjectionActor(_appName, aggregateId);
    }

    /// <inheritdoc/>
    public Task SetAsync<T>(string aggregateId, T state, CancellationToken cancellationToken)
        where T : class
    {
        ArgumentNullException.ThrowIfNullOrEmpty(aggregateId);
        return _actorFactory.GetCustomerProjectionActor(_appName, aggregateId).SetAsync(JsonSerializer.Serialize(state));
    }
}
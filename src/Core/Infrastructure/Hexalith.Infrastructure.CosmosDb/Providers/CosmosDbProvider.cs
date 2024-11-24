// <copyright file="CosmosDbProvider.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.CosmosDb.Providers;

using System;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

/// <summary>
/// Represents a provider for interacting with Cosmos DB.
/// </summary>
public class CosmosDbProvider : IDisposable
{
    /// <summary>
    /// The connection string.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// The container name.
    /// </summary>
    private readonly string _containerName;

    /// <summary>
    /// The database name.
    /// </summary>
    private readonly string _databaseName;

    /// <summary>
    /// The container.
    /// </summary>
    private Container? _container;

    /// <summary>
    /// The cosmos client.
    /// </summary>
    private CosmosClient? _cosmosClient;

    /// <summary>
    /// The database.
    /// </summary>
    private Database? _database;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosDbProvider"/> class.
    /// </summary>
    /// <param name="settings">The Cosmos DB settings.</param>
    public CosmosDbProvider(IOptions<CosmosDbSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<CosmosDbSettings>.ThrowIfNullOrWhiteSpace(settings.Value.ConnectionString);
        SettingsException<CosmosDbSettings>.ThrowIfNullOrWhiteSpace(settings.Value.DatabaseName);
        SettingsException<CosmosDbSettings>.ThrowIfNullOrWhiteSpace(settings.Value.ContainerName);
        _connectionString = settings.Value.ConnectionString;
        _databaseName = settings.Value.DatabaseName;
        _containerName = settings.Value.ContainerName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosDbProvider"/> class.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="databaseName">The database name.</param>
    /// <param name="containerName">The container name.</param>
    public CosmosDbProvider(string connectionString, string databaseName, string containerName)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionString);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(containerName);
        _connectionString = connectionString;
        _databaseName = databaseName;
        _containerName = containerName;
    }

    /// <summary>
    /// Gets the client.
    /// </summary>
    /// <value>The client.</value>
    public CosmosClient Client => _cosmosClient ??= new CosmosClient(_connectionString);

    /// <summary>
    /// Gets the container.
    /// </summary>
    /// <value>The container.</value>
    public Container Container => _container ??= Database.GetContainer(_containerName);

    /// <summary>
    /// Gets the database.
    /// </summary>
    /// <value>The database.</value>
    public Database Database => _database ??= Client.GetDatabase(_databaseName);

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (_cosmosClient != null)
            {
                _cosmosClient.Dispose();
                _cosmosClient = null;
            }
        }
    }
}
// <copyright file="UserPartitionServiceProxy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.ClientAppOnWasm.Services;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Sessions.Services;

/// <summary>
/// Service to manage user partitions.
/// </summary>
public class UserPartitionServiceProxy : IUserPartitionService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPartitionServiceProxy"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when the httpClient is null.</exception>
    public UserPartitionServiceProxy(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<string> GetDefaultPartitionAsync(string userName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userName);
        string? partition = await _httpClient.GetFromJsonAsync<string>($"api/UserPartition/{userName}/default", cancellationToken);
        return string.IsNullOrWhiteSpace(partition)
            ? throw new InvalidOperationException($"No default partition found for user {userName}")
            : partition;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetPartitionsAsync(string userName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userName);
        return await _httpClient.GetFromJsonAsync<IEnumerable<string>>($"api/UserPartition/{userName}/partitions", cancellationToken) ?? [];
    }

    /// <inheritdoc/>
    public async Task<bool> InPartitionAsync(string userName, string partitionId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userName);
        return await _httpClient.GetFromJsonAsync<bool>($"api/UserPartition/{userName}/in-partition/{partitionId}", cancellationToken);
    }
}
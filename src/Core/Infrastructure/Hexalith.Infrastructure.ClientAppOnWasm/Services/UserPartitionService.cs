﻿// <copyright file="UserPartitionService.cs" company="ITANEO">
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
public class UserPartitionService : IUserPartitionService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserPartitionService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client instance.</param>
    /// <exception cref="ArgumentNullException">Thrown when the httpClient is null.</exception>
    public UserPartitionService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    public async Task<string?> GetDefaultPartitionAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return await _httpClient.GetFromJsonAsync<string?>($"api/{nameof(GetDefaultPartitionAsync)}/{userId}", cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetPartitionsAsync(string userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return await _httpClient.GetFromJsonAsync<IEnumerable<string>>($"api/{nameof(GetPartitionsAsync)}/{userId}", cancellationToken) ?? [];
    }

    /// <inheritdoc/>
    public async Task<bool> InPartitionAsync(string userId, string partitionId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        return await _httpClient.GetFromJsonAsync<bool>($"api/{nameof(InPartitionAsync)}/{userId}", cancellationToken);
    }
}
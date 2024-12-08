﻿// <copyright file="IIdCollectionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Interface for a service that manages a collection of IDs.
/// </summary>
public interface IIdCollectionService
{
    /// <summary>
    /// Adds an aggregate global ID to the collection.
    /// </summary>
    /// <param name="aggregateGlobalId">The aggregate global ID to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddAsync(string aggregateGlobalId, CancellationToken cancellationToken);
}
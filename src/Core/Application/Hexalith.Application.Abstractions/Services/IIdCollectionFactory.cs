﻿// <copyright file="IIdCollectionFactory.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Services;

/// <summary>
/// Interface for creating ID collection services.
/// </summary>
public interface IIdCollectionFactory
{
    /// <summary>
    /// Gets the aggregate collection name.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>The collection name for the aggregate.</returns>
    static string GetAggregateCollectionName(string aggregateName) => $"{aggregateName}Ids";

    /// <summary>
    /// Creates the ID collection service asynchronously.
    /// </summary>
    /// <param name="collectionName">Name of the collection.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <returns>An instance of <see cref="IIdCollectionService"/>.</returns>
    IIdCollectionService CreateService(string collectionName, string partitionId);
}
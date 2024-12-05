// <copyright file="IUserPartitionService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Services;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for user partition service.
/// </summary>
public interface IUserPartitionService
{
    /// <summary>
    /// Gets the default partition for a user.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The default partition identifier.</returns>
    Task<string> GetDefaultPartitionAsync(string userName, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the partitions for a user.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of partition identifiers.</returns>
    Task<IEnumerable<string>> GetPartitionsAsync(string userName, CancellationToken cancellationToken);

    /// <summary>
    /// Determines whether the user is in a partition.
    /// </summary>
    /// <param name="userName">The user name.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><c>true</c> if the user is in a partition; otherwise, <c>false</c>.</returns>
    Task<bool> InPartitionAsync(string userName, string partitionId, CancellationToken cancellationToken);
}
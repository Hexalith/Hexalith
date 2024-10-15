// <copyright file="INumberSequenceService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.NumberSequences;

using System.Threading.Tasks;

/// <summary>
/// Interface INumberSequenceService.
/// </summary>
public interface INumberSequenceService
{
    /// <summary>
    /// Gets the next sequence.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;System.String&gt;.</returns>
    Task<string> GetNextAsync(
        string partitionId,
        string companyId,
        string id,
        CancellationToken cancellationToken);
}
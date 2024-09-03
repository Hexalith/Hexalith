// ***********************************************************************
// Assembly         :
// Author           : Jérôme Piquot
// Created          : 02-18-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-18-2024
// ***********************************************************************
// <copyright file="INumberSequenceService.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.NumberSequences.Abstractions.Application.Services;

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
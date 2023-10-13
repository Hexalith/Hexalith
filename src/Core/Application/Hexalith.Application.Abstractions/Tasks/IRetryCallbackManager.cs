// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-09-2023
// ***********************************************************************
// <copyright file="IRetryCallbackManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Tasks;

using System;
using System.Threading.Tasks;

/// <summary>
/// Interface IRetryCallbackManager.
/// </summary>
public interface IRetryCallbackManager
{
    /// <summary>
    /// Registers the continue callback asynchronous.
    /// </summary>
    /// <param name="dueTime">The due time.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task RegisterContinueCallbackAsync(TimeSpan dueTime, CancellationToken cancellationToken);

    /// <summary>
    /// Unregisters the continue callback asynchronous.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task.</returns>
    Task UnregisterContinueCallbackAsync(CancellationToken cancellationToken);
}
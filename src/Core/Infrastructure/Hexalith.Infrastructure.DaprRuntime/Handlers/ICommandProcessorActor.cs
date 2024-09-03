// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprHandlers
// Author           : Jérôme Piquot
// Created          : 02-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-12-2023
// ***********************************************************************
// <copyright file="ICommandProcessorActor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;

/// <summary>
/// Interface ICommandProcessorActor
/// Extends the <see cref="IActor" />.
/// </summary>
/// <seealso cref="IActor" />
public interface ICommandProcessorActor : IActor
{
    /// <summary>
    /// Continues the asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task ContinueAsync();

    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="envelope">The envelope.</param>
    /// <returns>Task.</returns>
    Task DoAsync(ActorCommandEnvelope envelope);

    /// <summary>
    /// Determines whether has commands.
    /// </summary>
    /// <returns>Returns true is has commands, else false.</returns>
    Task<bool> HasCommandsAsync();
}
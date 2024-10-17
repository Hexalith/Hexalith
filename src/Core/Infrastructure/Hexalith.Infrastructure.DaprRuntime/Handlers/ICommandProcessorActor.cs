// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprHandlers
// Author           : Jérôme Piquot
// Created          : 02-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-12-2023
// ***********************************************************************
// <copyright file="ICommandProcessorActor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Handlers;

using System.Threading.Tasks;

using Dapr.Actors;

using Hexalith.Infrastructure.DaprRuntime.Actors;

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
    /// Does a command execution.
    /// </summary>
    /// <param name="envelope">The envelope containing the command.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DoAsync(ActorMessageEnvelope envelope);

    /// <summary>
    /// Determines whether has commands.
    /// </summary>
    /// <returns>Returns true is has commands, else false.</returns>
    Task<bool> HasCommandsAsync();
}
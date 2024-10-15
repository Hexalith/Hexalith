// <copyright file="ICommandSaga.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

/// <summary>
/// The command saga interface.
/// </summary>
public interface ICommandSaga
{
    /// <summary>
    /// Completes the asynchronous.
    /// </summary>
    /// <returns>Task.</returns>
    Task CompleteAsync();
}
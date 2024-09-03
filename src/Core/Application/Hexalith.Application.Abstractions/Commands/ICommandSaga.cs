// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-16-2024
// ***********************************************************************
// <copyright file="ICommandSaga.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
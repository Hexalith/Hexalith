// <copyright file="ICommand.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Hexalith.Domain.Messages;

/// <summary>
/// Interface for all commands.
/// </summary>
public interface ICommand : IMessage
{
}
// <copyright file="IRequest.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using Hexalith.Domain.Messages;

/// <summary>
/// Interface for all commands.
/// </summary>
public interface IRequest : IMessage
{
}
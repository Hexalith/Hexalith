// <copyright file="ICommandBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Interface for all command buses.
/// </summary>
public interface ICommandBus : IMessageBus<ICommand, IMetadata>
{
}

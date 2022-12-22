// <copyright file="IEventBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Events;

using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Interface for all event buses.
/// </summary>
public interface IEventBus : IMessageBus<IEvent, IMetadata>
{
}

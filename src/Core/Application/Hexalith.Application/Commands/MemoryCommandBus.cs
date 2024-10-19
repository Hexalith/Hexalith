// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryCommandBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;

/// <summary>
/// Memory Command Bus.
/// </summary>
public class MemoryCommandBus : ICommandBus
{
    private readonly List<(object, Metadata)>? _messagestream;

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<MessageState> _stream = [];

    public List<(object, Metadata)> Messagestream => _messagestream ?? [];

    /// <inheritdoc/>
    public async Task PublishAsync(MessageState commandState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commandState);
        Messagestream.Add((commandState.Message, commandState.Metadata));
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(object command, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        Messagestream.Add((command, metadata));
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
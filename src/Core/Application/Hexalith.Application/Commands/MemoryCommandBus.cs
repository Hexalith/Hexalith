// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryCommandBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Common;

/// <summary>
/// Memory Command Bus.
/// </summary>
/// <param name="dateTimeService">The date time service.</param>
public class MemoryCommandBus(IDateTimeService dateTimeService) : ICommandBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<CommandState> _stream = [];

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<CommandState> Stream => _stream;

    /// <inheritdoc/>
    public async Task PublishAsync(IEnvelope<BaseCommand, BaseMetadata> envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        await PublishAsync(envelope.Message, envelope.Metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(BaseCommand command, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        await PublishAsync(new CommandState(_dateTimeService.UtcNow, command, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(CommandState commandState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commandState);
        _stream.Add(commandState);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
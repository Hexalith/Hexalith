// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryCommandBus.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
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
/// Class MemoryMessageBus.
/// Implements the <see cref="IMessageBus{TMessage, TMetadata}" />.
/// </summary>
/// <seealso cref="IMessageBus{TMessage, TMetadata}" />
public class MemoryCommandBus : ICommandBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<CommandState> _stream = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryCommandBus"/> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    public MemoryCommandBus(IDateTimeService dateTimeService) => _dateTimeService = dateTimeService;

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<CommandState> Stream => _stream;

    /// <inheritdoc/>
    public Task PublishAsync(IEnvelope<BaseCommand, BaseMetadata> envelope, CancellationToken cancellationToken) => PublishAsync(envelope.Message, envelope.Metadata, cancellationToken);

    /// <inheritdoc/>
    public Task PublishAsync(BaseCommand message, BaseMetadata metadata, CancellationToken cancellationToken) => PublishAsync(new CommandState(_dateTimeService.UtcNow, message, metadata), cancellationToken);

    /// <inheritdoc/>
    public Task PublishAsync(CommandState state, CancellationToken cancellationToken)
    {
        _stream.Add(state);
        return Task.CompletedTask;
    }
}
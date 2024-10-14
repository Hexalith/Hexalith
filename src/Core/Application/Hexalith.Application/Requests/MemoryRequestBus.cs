// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryRequestBus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Requests;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Extensions.Common;

/// <summary>
/// Memory Request Bus.
/// </summary>
/// <param name="dateTimeService">The date time service.</param>
public class MemoryRequestBus(TimeProvider dateTimeService) : IRequestBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly TimeProvider _dateTimeService = dateTimeService;

    private readonly List<(object, MessageMetadatas.Metadata)> _messageStream = [];

    /// <summary>
    /// The stream.
    /// </summary>
    private readonly List<RequestState> _stream = [];

    /// <summary>
    /// Gets the stream.
    /// </summary>
    /// <value>The stream.</value>
    public IEnumerable<RequestState> Stream => _stream;

    /// <inheritdoc/>
    public async Task PublishAsync(IEnvelope<BaseRequest, BaseMetadata> envelope, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(envelope);
        await PublishAsync(envelope.Message, envelope.Metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(BaseRequest request, BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);
        await PublishAsync(new RequestState(_dateTimeService.GetUtcNow(), request, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(RequestState requestState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(requestState);
        _stream.Add(requestState);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task PublishAsync(object message, MessageMetadatas.Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        ArgumentNullException.ThrowIfNull(metadata);
        _messageStream.Add((message, metadata));
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task PublishAsync(MessageMetadatas.MessageState message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(message);
        _messageStream.Add((message.Message, message.Metadata));
        return Task.CompletedTask;
    }
}
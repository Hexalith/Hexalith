// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-04-2023
// ***********************************************************************
// <copyright file="MemoryRequestBus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
public class MemoryRequestBus(IDateTimeService dateTimeService) : IRequestBus
{
    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly IDateTimeService _dateTimeService = dateTimeService;

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
        await PublishAsync(new RequestState(_dateTimeService.UtcNow, request, metadata), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task PublishAsync(RequestState requestState, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(requestState);
        _stream.Add(requestState);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
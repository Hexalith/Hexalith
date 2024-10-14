// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="BusCommandProcessor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Extensions.Common;

/// <summary>
/// Class BusCommandProcessor.
/// Implements the <see cref="Hexalith.Application.Commands.ICommandProcessor" />.
/// </summary>
/// <seealso cref="Hexalith.Application.Commands.ICommandProcessor" />
public class BusCommandProcessor : ICommandProcessor
{
    private readonly ICommandBus _bus;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusCommandProcessor"/> class.
    /// </summary>
    /// <param name="bus">The bus.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public BusCommandProcessor([NotNull] ICommandBus bus, [NotNull] TimeProvider dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(bus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _bus = bus;
    }

    /// <inheritdoc/>
    public async Task SubmitAsync([NotNull] BaseCommand command, [NotNull] Metadatas.BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        await SubmitAsync([command], metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SubmitAsync([NotNull] IEnumerable<BaseCommand> commands, [NotNull] Metadatas.BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(metadata);
        foreach (BaseCommand command in commands)
        {
            await _bus
                .PublishAsync(command, Metadatas.Metadata.CreateNew(command, metadata), cancellationToken)
                .ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task SubmitAsync(object command, [NotNull] Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        await _bus
            .PublishAsync(command, metadata, cancellationToken)
            .ConfigureAwait(false);
    }

    public Task SubmitAsync(object command, Metadatas.Metadata metadata, CancellationToken cancellationToken) => throw new NotImplementedException();
}
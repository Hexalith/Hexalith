// <copyright file="BusCommandProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Applications.Commands;
using Hexalith.Commons.Metadatas;

/// <summary>
/// Processes commands by submitting them to a command bus.
/// </summary>
public class BusCommandProcessor : IDomainCommandProcessor
{
    private readonly ICommandBus _bus;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusCommandProcessor"/> class.
    /// </summary>
    /// <param name="bus">The bus to which commands will be submitted.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="bus"/> or <paramref name="dateTimeService"/> is null.</exception>
    public BusCommandProcessor([NotNull] ICommandBus bus, [NotNull] TimeProvider dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(bus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _bus = bus;
    }

    /// <summary>
    /// Submits a command asynchronously to the command bus.
    /// </summary>
    /// <param name="command">The command to be submitted.</param>
    /// <param name="metadata">The metadata associated with the command.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command"/> or <paramref name="metadata"/> is null.</exception>
    public async Task SubmitAsync(object command, [NotNull] Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        await _bus
            .PublishAsync(command, metadata, cancellationToken)
            .ConfigureAwait(false);
    }
}
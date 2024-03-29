﻿// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="BusCommandProcessor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
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
    public BusCommandProcessor([NotNull] ICommandBus bus, [NotNull] IDateTimeService dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(bus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _bus = bus;
    }

    /// <inheritdoc/>
    public async Task SubmitAsync([NotNull] BaseCommand command, [NotNull] BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        await SubmitAsync([command], metadata, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SubmitAsync([NotNull] IEnumerable<BaseCommand> commands, [NotNull] BaseMetadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(commands);
        ArgumentNullException.ThrowIfNull(metadata);
        foreach (BaseCommand command in commands)
        {
            await _bus
                .PublishAsync(command, Metadata.CreateNew(command, metadata), cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
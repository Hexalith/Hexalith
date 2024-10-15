// ***********************************************************************
// Assembly         : Hexalith.Application
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="BusCommandProcessor.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;


public class BusCommandProcessor : IDomainCommandProcessor
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
    public async Task SubmitAsync(object command, [NotNull] Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(metadata);
        await _bus
            .PublishAsync(command, metadata, cancellationToken)
            .ConfigureAwait(false);
    }
}
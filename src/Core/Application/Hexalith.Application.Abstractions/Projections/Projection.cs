// <copyright file="Projection.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Represents an abstract base class for projections in the application.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IProjection"/> interface and provides
/// abstract methods for executing projections and handling domain events.
/// </remarks>
public abstract class Projection : IProjection
{
    /// <inheritdoc/>
    public abstract Task<object> ExecuteAsync(object request, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task HandleAsync(object domainEvent, CancellationToken cancellationToken);
}
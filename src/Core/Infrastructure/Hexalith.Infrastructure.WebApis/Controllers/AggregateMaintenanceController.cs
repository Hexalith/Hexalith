// <copyright file="AggregateMaintenanceController.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Controllers;

using System.Threading.Tasks;

using Hexalith.Application.Aggregates;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base controller for aggregate maintenance operations.
/// </summary>
[ApiController]
[Route("api/maintenance")]
public class AggregateMaintenanceController : ControllerBase
{
    private readonly IAggregateMaintenance _aggregateMaintenance;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateMaintenanceController"/> class.
    /// </summary>
    /// <param name="aggregateMaintenance">Aggregate maintenance service.</param>
    protected AggregateMaintenanceController(IAggregateMaintenance aggregateMaintenance)
    {
        ArgumentNullException.ThrowIfNull(aggregateMaintenance);
        _aggregateMaintenance = aggregateMaintenance;
    }

    /// <summary>
    /// Clears the commands of all aggregates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost("commands/clear")]
    public async Task<ActionResult> ClearAllCommandsAsync(CancellationToken cancellationToken)
    {
        await _aggregateMaintenance.ClearAllCommandsAsync(cancellationToken).ConfigureAwait(false);
        return Ok();
    }

    /// <summary>
    /// Clears the commands of the aggregate with the specified ID.
    /// </summary>
    /// <param name="aggregateId">The ID of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost("commands/clear/{aggregateId}")]
    public async Task<ActionResult> ClearCommandsAsync(string? aggregateId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(aggregateId))
        {
            return BadRequest("AggregateId is required.");
        }

        await _aggregateMaintenance.ClearCommandsAsync(aggregateId, cancellationToken).ConfigureAwait(false);
        return Ok();
    }

    /// <summary>
    /// Sends snapshots of all aggregates.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost("snapshot")]
    public async Task<ActionResult> SendAllSnapshotsAsync(CancellationToken cancellationToken)
    {
        await _aggregateMaintenance.SendAllSnapshotsAsync(cancellationToken).ConfigureAwait(false);
        return Ok();
    }

    /// <summary>
    /// Sends a snapshot of the aggregate with the specified ID.
    /// </summary>
    /// <param name="aggregateId">The ID of the aggregate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost("snapshot/{aggregateId}")]
    public async Task<ActionResult> SendSnapshotAsync(string aggregateId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(aggregateId))
        {
            return BadRequest("AggregateId is required.");
        }

        await _aggregateMaintenance.SendSnapshotAsync(aggregateId, cancellationToken).ConfigureAwait(false);
        return Ok();
    }
}
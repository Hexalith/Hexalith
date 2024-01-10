// ***********************************************************************
// Assembly         : Hexalith.Server.Sales
// Author           : Jérôme Piquot
// Created          : 01-04-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-04-2024
// ***********************************************************************
// <copyright file="TestController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Server.Sales.Infrastructure.Controllers;

using Dapr.Actors;
using Dapr.Actors.Client;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Sales.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Handlers;
using Hexalith.Infrastructure.DaprRuntime.Sales.Actors;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Class TerstController.
/// Implements the <see cref="ControllerBase" />.
/// </summary>
/// <seealso cref="ControllerBase" />
[ApiController]
public class TestController : ControllerBase
{
    /// <summary>
    /// The proxy factory.
    /// </summary>
    private readonly IActorProxyFactory _proxyFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestController"/> class.
    /// </summary>
    /// <param name="proxyFactory">The proxy factory.</param>
    public TestController(IActorProxyFactory proxyFactory) => _proxyFactory = proxyFactory;

    /// <summary>
    /// Test actor as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;ActionResult&gt; representing the asynchronous operation.</returns>
    [HttpGet("/hello")]
    public async Task<ActionResult> TestActorAsync()
    {
        IssueSalesInvoice command = new(
            "FFY",
            "FRRT",
            "EDI",
            "123456",
            DateTimeOffset.UtcNow,
            "CUST123",
            "EUR",
            [new SalesInvoiceLine(
                    "1",
                    new SalesLineItem("Item1", 10.5m, "UNIT", 123.25m),
                    new SalesLineOrigin("W1", "VEN123"),
                    [new SalesLineTax("V20", "VAT 20%", 10.56m)])]);
        Metadata metadata = new(
            UniqueIdHelper.GenerateUniqueStringId(),
            command,
            DateTimeOffset.UtcNow,
            new ContextMetadata(UniqueIdHelper.GenerateUniqueStringId(), "test-user", DateTimeOffset.UtcNow, null, null),
            null);

        IAggregateActor actor = _proxyFactory.CreateActorProxy<IAggregateActor>(
            new ActorId(command.AggregateId),
            AggregateActor.GetAggregateActorName(SalesInvoice.GetAggregateName()));

        await actor
            .SubmitCommandAsync(new ActorCommandEnvelope([command], [metadata]))
            .ConfigureAwait(false);
        _ = await actor.ProcessNextCommandAsync().ConfigureAwait(false);
        _ = await actor.PublishNextMessageAsync().ConfigureAwait(false);
        return Ok(metadata.Message.Id);
    }
}
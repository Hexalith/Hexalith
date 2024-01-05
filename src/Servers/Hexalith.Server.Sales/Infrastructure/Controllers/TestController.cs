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
        ISalesInvoiceAggregateActor actor = _proxyFactory.CreateActorProxy<ISalesInvoiceAggregateActor>(new ActorId("Test1"), nameof(SalesInvoiceAggregateActor));
        string hello = await actor.SayHelloAsync().ConfigureAwait(false);
        return Ok(hello);
    }
}
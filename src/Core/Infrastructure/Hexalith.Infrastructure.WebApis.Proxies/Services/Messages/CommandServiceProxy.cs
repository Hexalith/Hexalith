// <copyright file="CommandServiceProxy.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Proxies.Services.Messages;

using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Application.States;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a proxy for the command service.
/// </summary>
public class CommandServiceProxy : ServiceApiProxy, ICommandService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandServiceProxy"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for making requests.</param>
    /// <param name="logger">The logger used for logging.</param>
    public CommandServiceProxy(HttpClient httpClient, ILogger<CommandServiceProxy> logger)
        : base(httpClient, logger)
    {
    }

    /// <inheritdoc/>
    public async Task SubmitCommandAsync(CommandState command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync("/Command/Submit", command, cancellationToken)
            .ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
    }
}
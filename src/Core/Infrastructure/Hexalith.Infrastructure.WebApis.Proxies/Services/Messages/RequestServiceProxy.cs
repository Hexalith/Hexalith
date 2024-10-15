// <copyright file="RequestServiceProxy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Proxies.Services.Messages;

using System;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Application.Requests;

using Microsoft.Extensions.Logging;

/// <summary>
/// Represents a proxy for the request service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RequestServiceProxy"/> class.
/// </remarks>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="logger">The logger.</param>
public class RequestServiceProxy(HttpClient httpClient, ILogger<RequestServiceProxy> logger) : ServiceApiProxy(httpClient, logger), IRequestService
{
    /// <inheritdoc/>
    public async Task SubmitRequestAsync(MessageState request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync("/Request/Submit", request, cancellationToken)
            .ConfigureAwait(false);
        _ = response.EnsureSuccessStatusCode();
    }
}
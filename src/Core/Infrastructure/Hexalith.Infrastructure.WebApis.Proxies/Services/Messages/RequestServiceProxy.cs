// <copyright file="RequestServiceProxy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Proxies.Services.Messages;

using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Requests;
using Hexalith.Application.Sessions.Services;
using Hexalith.PolymorphicSerialization;

using Microsoft.Extensions.Logging;

/// <summary>
/// Proxy service for handling requests.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RequestServiceProxy"/> class.
/// </remarks>
/// <param name="userPartitionService">The user partition service.</param>
/// <param name="httpClient">The HTTP client.</param>
/// <param name="timeProvider">The time provider.</param>
/// <param name="logger">The logger.</param>
public partial class RequestServiceProxy(
    IUserPartitionService userPartitionService,
    HttpClient httpClient,
    TimeProvider timeProvider,
    ILogger<RequestServiceProxy> logger) : ServiceApiProxy(userPartitionService, httpClient, timeProvider, logger), IRequestService
{
    /// <inheritdoc/>
    public async Task<TRequest> SubmitAsync<TRequest>(ClaimsPrincipal user, TRequest request, CancellationToken cancellationToken)
                where TRequest : PolymorphicRecordBase
        => await PostAsync<TRequest, TRequest>("api/request/submit", user, request, cancellationToken);
}
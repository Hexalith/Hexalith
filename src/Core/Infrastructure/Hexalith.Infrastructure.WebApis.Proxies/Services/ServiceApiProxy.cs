// <copyright file="ServiceApiProxy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Proxies.Services;

using System;

using Microsoft.Extensions.Logging;

/// <summary>
/// Http API service proxy base class.
/// </summary>
public class ServiceApiProxy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceApiProxy"/> class.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> instance used for making HTTP requests.</param>
    /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
    public ServiceApiProxy(HttpClient httpClient, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);
        HttpClient = httpClient;
        Logger = logger;
    }

    /// <summary>
    /// Gets the <see cref="HttpClient"/> instance used for making HTTP requests.
    /// </summary>
    protected HttpClient HttpClient { get; }

    /// <summary>
    /// Gets the <see cref="ILogger"/> instance used for logging.
    /// </summary>
    protected ILogger Logger { get; }
}
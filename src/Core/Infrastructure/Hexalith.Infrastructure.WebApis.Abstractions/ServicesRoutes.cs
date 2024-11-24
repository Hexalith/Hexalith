// <copyright file="ServicesRoutes.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents the routes for the services.
/// </summary>
[SuppressMessage(
    "Critical Code Smell",
    "S2339:Public constant members should not be used",
    Justification = "Attribute values need constant values")]
public static class ServicesRoutes
{
    /// <summary>
    /// Represents the route for the command service.
    /// </summary>
    public const string CommandService = "api/command";

    /// <summary>
    /// Represents the route for publishing a request.
    /// </summary>
    public const string PublishRequest = "publish";

    /// <summary>
    /// Represents the route for the request service.
    /// </summary>
    public const string RequestService = "api/request";

    /// <summary>
    /// Represents the route for submitting a command.
    /// </summary>
    public const string SubmitCommand = "submit";

    /// <summary>
    /// Represents the route for submitting a request.
    /// </summary>
    public const string SubmitRequest = "submit";
}
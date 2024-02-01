// <copyright file="ServicesRoutes.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis;

/// <summary>
/// Represents the routes for the services.
/// </summary>
public static class ServicesRoutes
{
    /// <summary>
    /// Represents the route for the command service.
    /// </summary>
    public const string CommandService = "/Command";

    /// <summary>
    /// Represents the route for submitting a command.
    /// </summary>
    public const string PublishCommand = "/Command/Publish";

    /// <summary>
    /// Represents the route for publishing a request.
    /// </summary>
    public const string PublishRequest = "/Request/Publish";

    /// <summary>
    /// Represents the route for the request service.
    /// </summary>
    public const string RequestService = "/Request";

    /// <summary>
    /// Represents the route for submitting a request.
    /// </summary>
    public const string SubmitRequest = "/Request/Submit";
}
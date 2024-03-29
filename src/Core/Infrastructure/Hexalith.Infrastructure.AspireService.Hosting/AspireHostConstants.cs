// <copyright file="AspireHostConstants.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting;

/// <summary>
/// Class ApplicationConstants.
/// </summary>
internal static class AspireHostConstants
{
    /// <summary>
    /// The command bus component name.
    /// </summary>
    public const string CommandBusComponentName = "command-bus";

    /// <summary>
    /// The deployment name.
    /// </summary>
    public const string DeploymentName = nameof(DeploymentName);

    /// <summary>
    /// The event bus component name.
    /// </summary>
    public const string EventBusComponentName = "event-bus";

    /// <summary>
    /// The global secret store connection string name.
    /// </summary>
    public const string GlobalSecretStoreConnectionStringName = "GlobalSecretStore";

    /// <summary>
    /// The notification bus component name.
    /// </summary>
    public const string NotificationBusComponentName = "notification-bus";

    /// <summary>
    /// The request bus component name.
    /// </summary>
    public const string RequestBusComponentName = "request-bus";

    /// <summary>
    /// The secret store component name.
    /// </summary>
    public const string SecretStoreComponentName = "secretstore";
}
// <copyright file="AspireApplicationConstants.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting;

/// <summary>
/// Class ApplicationConstants.
/// </summary>
internal static class AspireApplicationConstants
{
    /// <summary>
    /// The deployment name.
    /// </summary>
    public const string DeploymentName = nameof(DeploymentName);

    /// <summary>
    /// The global secret store connection string name.
    /// </summary>
    public const string GlobalSecretStoreConnectionStringName = "GlobalSecretStore";

    /// <summary>
    /// The notification bus component name.
    /// </summary>
    public const string NotificationBusComponentName = "notifications";

    /// <summary>
    /// The Dapr component publish sub folder.
    /// </summary>
    public const string PublishPath = "PublishMode";

    /// <summary>
    /// The request bus component name.
    /// </summary>
    public const string RequestBusComponentName = "requests";

    /// <summary>
    /// The Dapr component run sub folder.
    /// </summary>
    public const string RunPath = "RunMode";

    /// <summary>
    /// The secret store component name.
    /// </summary>
    public const string SecretStoreComponentName = "secretstore";
}
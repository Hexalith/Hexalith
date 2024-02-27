// <copyright file="HexalithProjectHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.AspireHost.Helpers;

using System.Collections.Immutable;

using Aspire.Hosting.Dapr;

/// <summary>
/// Class HexalithProjectHelper.
/// </summary>
public static class HexalithProjectHelper
{
    /// <summary>
    /// Adds the hexalith project.
    /// </summary>
    /// <typeparam name="TProject">The type of the t project.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <param name="index">The index.</param>
    /// <returns>IResourceBuilder&lt;ProjectResource&gt;.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IResourceBuilder<ProjectResource> AddHexalithProject<TProject>(
        this IDistributedApplicationBuilder builder)
        where TProject : IProjectMetadata, new()
    {
        ArgumentNullException.ThrowIfNull(builder);

        // _ = 8080 + index;
        TProject project = new();
#pragma warning disable CA1308 // Normalize strings to uppercase
        string appName = project.ProjectPath.Split('.').Skip(1).Last().ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
        return builder
            .AddProject<TProject>(appName)

             // .WithHttpEndpoint(
             //    8080,
             //    appPort,
             //    appName)
             .WithDaprSidecar(new DaprSidecarOptions
             {
                 // AppId = appName,
                 // AppProtocol = "http",
                 ResourcesPaths = ImmutableHashSet.Create($"{project.ProjectPath}/Infrastructure/Components"),
                 Config = $"{project.ProjectPath}/Infrastructure/Components/config.yaml",

                 // MetricsPort = 9090 + index,
                 // DaprHttpPort = 3500 + index,
                 // DaprGrpcPort = 50000 + index,
                 // AppPort = appPort,
             });
    }
}
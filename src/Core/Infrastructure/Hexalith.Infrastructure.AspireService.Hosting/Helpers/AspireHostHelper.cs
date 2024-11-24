// <copyright file="AspireHostHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AspireService.Hosting.Helpers;

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;

/// <summary>
/// Helper class for managing Aspire host configuration.
/// </summary>
public static class AspireHostHelper
{
    /// <summary>
    /// Adds a hook to set the ASPNETCORE_FORWARDEDHEADERS_ENABLED environment variable to true for all projects in the application.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IDistributedApplicationBuilder AddForwardedHeaders([NotNull] this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.TryAddLifecycleHook<AddForwardHeadersHook>();
        return builder;
    }

    private class AddForwardHeadersHook : IDistributedApplicationLifecycleHook
    {
        /// <summary>
        /// Executes before the application starts.
        /// </summary>
        /// <param name="appModel">The distributed application model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task BeforeStartAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
        {
            foreach (ProjectResource p in appModel.GetProjectResources())
            {
                p.Annotations.Add(new EnvironmentCallbackAnnotation(context => context.EnvironmentVariables["ASPNETCORE_FORWARDEDHEADERS_ENABLED"] = "true"));
            }

            return Task.CompletedTask;
        }
    }
}
// <copyright file="GraphQLHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.GraphQLServer.Helpers;

using HotChocolate.Execution.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Provides helper methods for configuring GraphQL services.
/// </summary>
public static class GraphQLHelper
{
    /// <summary>
    /// Adds GraphQL services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="builder">The application builder to add GraphQL services to.</param>
    /// <returns>The service collection with the GraphQL services added.</returns>
    public static IRequestExecutorBuilder AddHexalithGraphQL(this IHostApplicationBuilder builder)
        => builder
            .AddGraphQL()
            .AddGraphQLServerTypes();

    // .AddMutationType()
    // .AddSubscriptionType()
    // .AddType<UploadType>()
    // .AddGlobalObjectIdentification()
    // .AddMutationConventions();

    /// <summary>
    /// Configures the application to use GraphQL.
    /// </summary>
    /// <param name="app">The application builder to configure.</param>
    /// <returns>The application builder with GraphQL configured.</returns>
    public static IEndpointRouteBuilder UseHexalithGraphQL(this IEndpointRouteBuilder app)
    {
        _ = app.MapGraphQL();
        return app;
    }
}
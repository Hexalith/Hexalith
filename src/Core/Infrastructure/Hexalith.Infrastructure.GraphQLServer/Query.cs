// <copyright file="Query.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.GraphQLServer;

/// <summary>
/// Represents a query for saying hello.
/// </summary>
[QueryType]
public static class Query
{
    /// <summary>
    /// Gets a hello message.
    /// </summary>
    /// <param name="name">The name to include in the hello message.</param>
    /// <returns>A hello message.</returns>
    public static string GetHello([ID("Hello")] string name) => $"Hello {name}!";
}
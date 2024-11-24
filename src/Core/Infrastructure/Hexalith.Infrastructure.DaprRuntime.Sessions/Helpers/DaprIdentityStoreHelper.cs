// <copyright file="DaprIdentityStoreHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

using Microsoft.AspNetCore.Identity;

/// <summary>
/// Provides extension methods for adding Dapr stores to the IdentityBuilder.
/// </summary>
public static class DaprIdentityStoreHelper
{
    /// <summary>
    /// Adds Dapr stores to the specified IdentityBuilder.
    /// </summary>
    /// <param name="builder">The IdentityBuilder to add the Dapr stores to.</param>
    /// <returns>The IdentityBuilder with the Dapr stores added.</returns>
    public static IdentityBuilder AddDaprStores(this IdentityBuilder builder) => builder;
}
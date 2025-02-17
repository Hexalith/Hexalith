﻿// <copyright file="SessionActorsHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Sessions.Helpers;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application.Sessions.Services;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Actors;
using Hexalith.Infrastructure.DaprRuntime.Sessions.Services;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides helper methods for partition actors.
/// </summary>
public static class SessionActorsHelper
{
    /// <summary>
    /// Adds partition services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <returns>The updated IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddSessionsServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddSingleton<IUserIdentityService, UserIdentityService>();
        return services;
    }

    /// <summary>
    /// Registers partition actors with the specified ActorRegistrationCollection.
    /// </summary>
    /// <param name="actorRegistrationCollection">The ActorRegistrationCollection to register actors with.</param>
    /// <exception cref="ArgumentNullException">Thrown when actorRegistrationCollection is null.</exception>
    public static void RegisterSessionActors(this ActorRegistrationCollection actorRegistrationCollection)
    {
        ArgumentNullException.ThrowIfNull(actorRegistrationCollection);
        actorRegistrationCollection.RegisterActor<UserIdentityActor>(ActorProxyHelper.UserIdentityActorName);
        actorRegistrationCollection.RegisterActor<KeyHashActor>(ActorProxyHelper.UserIdentityCollectionName);
        actorRegistrationCollection.RegisterActor<SessionActor>(ActorProxyHelper.SessionActorName);
        actorRegistrationCollection.RegisterActor<UserActiveSessionActor>(ActorProxyHelper.UserActiveSessionActorName);
    }
}
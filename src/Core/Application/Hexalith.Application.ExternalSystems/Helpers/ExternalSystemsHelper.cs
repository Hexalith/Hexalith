// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-29-2023
// ***********************************************************************
// <copyright file="ExternalSystemsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.ExternalSystems.Helpers;

using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Hexalith.Application.Commands;
using Hexalith.Application.ExternalSystems.CommandHandlers;
using Hexalith.Application.ExternalSystems.Commands;
using Hexalith.Domain.Events;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ExternalSystemsHelper.
/// </summary>
public static class ExternalSystemsHelper
{
    /// <summary>
    /// Adds the external systems handlers.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddExternalSystemsCommandHandlers([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddScoped<ICommandHandler<AddExternalSystemReference>, AddExternalSystemReferenceHandler>();
        services.TryAddScoped<ICommandHandler<RemoveExternalSystemReference>, RemoveExternalSystemReferenceHandler>();
        return services
            .AddExternalSystemsEventValidators();
    }

    /// <summary>
    /// Adds the external systems event validators.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddExternalSystemsEventValidators([NotNull] this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        services.TryAddSingleton<IValidator<ExternalSystemReferenceAdded>, ExternalSystemReferenceAddedValidator>();
        services.TryAddSingleton<IValidator<ExternalSystemReferenceRemoved>, ExternalSystemReferenceRemovedValidator>();
        return services;
    }
}
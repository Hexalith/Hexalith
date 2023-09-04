// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="ExternalSystemsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.ExternalSystems.Helpers;

using Hexalith.Application.Commands;
using Hexalith.Application.ExternalSystems.CommandHandlers;
using Hexalith.Application.ExternalSystems.Commands;

using Microsoft.Extensions.DependencyInjection;

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
    public static IServiceCollection AddExternalSystemsCommandHandlers(this IServiceCollection services)
    {
        return services
            .AddTransient<ICommandHandler<AddAggregateExternalReference>, AddAggregateExternalReferenceHandler>()
            .AddTransient<ICommandHandler<RemoveAggregateExternalReference>, RemoveAggregateExternalReferenceHandler>()
            .AddTransient<ICommandHandler<AddExternalSystemReference>, AddExternalSystemReferenceHandler>()
            .AddTransient<ICommandHandler<RemoveExternalSystemReference>, RemoveExternalSystemReferenceHandler>();
    }
}
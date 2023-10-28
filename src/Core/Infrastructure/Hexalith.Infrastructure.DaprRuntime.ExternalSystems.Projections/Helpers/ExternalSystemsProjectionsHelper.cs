// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="ExternalSystemsProjectionsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Helpers;

using System.Diagnostics.CodeAnalysis;

using Dapr.Actors;
using Dapr.Actors.Client;
using Dapr.Actors.Runtime;

using Hexalith.Application;
using Hexalith.Application.ExternalSystems.Helpers;
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Configurations;
using Hexalith.Infrastructure.DaprRuntime.ExternalSystems.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Class ExternalSystemsHelper.
/// </summary>
public static class ExternalSystemsProjectionsHelper
{
    /// <summary>
    /// Adds the external systems service.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDaprExternalSystems([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services
            .AddExternalSystemsCommandHandlers()
            .ConfigureSettings<ExternalSystemsProjectionsSettings>(configuration)
            .TryAddTransient<IExternalReferenceMapperService>();
        services.TryAddTransient<IExternalReferenceMapperService, ExternalReferenceMapperService>();
        return services;
    }

    /// <summary>
    /// Adds the dapr external systems mapper.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDaprExternalSystemsMapper([NotNull] this IServiceCollection services, [NotNull] string applicationName, [NotNull] string aggregateName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrEmpty(aggregateName);
        services.TryAddSingleton<IExternalReferenceMapperService>(new ExternalReferenceMapperService(applicationName, aggregateName));
        return services;
    }

    /// <summary>
    /// Adds the external systems mapper.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddExternalSystemsMapper([NotNull] this ActorRegistrationCollection actors, [NotNull] string applicationName, [NotNull] string aggregateName)
    {
        ArgumentNullException.ThrowIfNull(actors);
        ArgumentException.ThrowIfNullOrEmpty(aggregateName);
        actors.RegisterActor<KeyValueActor>(GetAggregateToExternalReferenceActorName(applicationName, aggregateName));
        actors.RegisterActor<KeyValueActor>(GetExternalReferenceToAggregateActorName(applicationName, aggregateName));
        return actors;
    }

    /// <summary>
    /// Creates the external reference mapper identifier.
    /// </summary>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <returns>System.String.</returns>
    public static string CreateExternalReferenceMapperId(string aggregateId, string systemId)
        => aggregateId + ApplicationConstants.IdPartSeparator + systemId;

    /// <summary>
    /// Creates the external reference mapper identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <returns>System.String.</returns>
    public static string CreateExternalReferenceMapperId(string partitionId, string companyId, string systemId, string externalId)
            => partitionId + ApplicationConstants.IdPartSeparator + companyId + ApplicationConstants.IdPartSeparator + systemId + ApplicationConstants.IdPartSeparator + externalId;

    /// <summary>
    /// Gets the aggregate to external reference actor.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    public static IKeyValueActor GetAggregateToExternalReferenceActor(string applicationName, string aggregateName, string aggregateId, string systemId)
    {
        return ActorProxy.Create<IKeyValueActor>(
            new ActorId(CreateExternalReferenceMapperId(aggregateId, systemId)),
            GetAggregateToExternalReferenceActorName(applicationName, aggregateName));
    }

    /// <summary>
    /// Gets the name of the aggregate to external reference actor.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateToExternalReferenceActorName(string applicationName, string aggregateName) => applicationName + aggregateName + "ToExternalReference";

    /// <summary>
    /// Gets the external reference to aggregate actor.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    public static IKeyValueActor GetExternalReferenceToAggregateActor(string applicationName, string aggregateName, string partitionId, string companyId, string systemId, string externalId)
    {
        return ActorProxy.Create<IKeyValueActor>(
            new ActorId(CreateExternalReferenceMapperId(partitionId, companyId, systemId, externalId)),
            GetAggregateToExternalReferenceActorName(applicationName, aggregateName));
    }

    /// <summary>
    /// Gets the name of the external reference to aggregate actor.
    /// </summary>
    /// <param name="applicationName">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>System.String.</returns>
    public static string GetExternalReferenceToAggregateActorName(string applicationName, string aggregateName) => applicationName + "ExternalReferenceTo" + aggregateName;
}
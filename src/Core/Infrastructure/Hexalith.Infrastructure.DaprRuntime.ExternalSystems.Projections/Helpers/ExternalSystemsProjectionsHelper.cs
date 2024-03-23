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
using Hexalith.Application.ExternalSystems.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.DaprRuntime.Abstractions.Actors;
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
    /// Adds the dapr external systems mapper.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateNames">The aggregate names.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static IServiceCollection AddDaprExternalSystemsMapper(
        [NotNull] this IServiceCollection services,
        [NotNull] IConfiguration configuration,
        [NotNull] string applicationId)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        services
            .ConfigureSettings<ExternalSystemsProjectionsSettings>(configuration)
            .TryAddScoped<IExternalReferenceMapperService>(s => new ExternalReferenceMapperService(applicationId));
        return services;
    }

    /// <summary>
    /// Adds the external systems mapper.
    /// </summary>
    /// <param name="actors">The actors.</param>
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateNames">The aggregate names.</param>
    /// <returns>ActorRegistrationCollection.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public static ActorRegistrationCollection AddExternalSystemsMapper(
        [NotNull] this ActorRegistrationCollection actors,
        [NotNull] string applicationId,
        [NotNull] IEnumerable<string> aggregateNames)
    {
        ArgumentNullException.ThrowIfNull(actors);
        ArgumentNullException.ThrowIfNull(aggregateNames);
        foreach (string aggregateName in aggregateNames)
        {
            actors.RegisterActor<KeyValueActor>(GetAggregateToExternalReferenceActorName(applicationId, aggregateName));
            actors.RegisterActor<KeyValueActor>(GetExternalReferenceToAggregateActorName(applicationId, aggregateName));
        }

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
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="aggregateId">The aggregate identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    public static IKeyValueActor GetAggregateToExternalReferenceActor(string applicationId, string aggregateName, string aggregateId, string systemId)
    {
        return ActorProxy.Create<IKeyValueActor>(
            new ActorId(CreateExternalReferenceMapperId(aggregateId, systemId)),
            GetAggregateToExternalReferenceActorName(applicationId, aggregateName));
    }

    /// <summary>
    /// Gets the name of the aggregate to external reference actor.
    /// </summary>
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>System.String.</returns>
    public static string GetAggregateToExternalReferenceActorName(string applicationId, string aggregateName) => applicationId + aggregateName + "ToExternalReference";

    /// <summary>
    /// Gets the external reference to aggregate actor.
    /// </summary>
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="companyId">The company identifier.</param>
    /// <param name="systemId">The system identifier.</param>
    /// <param name="externalId">The external identifier.</param>
    /// <returns>IKeyValueActor.</returns>
    public static IKeyValueActor GetExternalReferenceToAggregateActor(string applicationId, string aggregateName, string partitionId, string companyId, string systemId, string externalId)
    {
        return ActorProxy.Create<IKeyValueActor>(
            new ActorId(CreateExternalReferenceMapperId(partitionId, companyId, systemId, externalId)),
            GetAggregateToExternalReferenceActorName(applicationId, aggregateName));
    }

    /// <summary>
    /// Gets the name of the external reference to aggregate actor.
    /// </summary>
    /// <param name="applicationId">Name of the application.</param>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>System.String.</returns>
    public static string GetExternalReferenceToAggregateActorName(string applicationId, string aggregateName)
        => applicationId + "ExternalReferenceTo" + aggregateName;
}
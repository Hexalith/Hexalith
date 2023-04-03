// ***********************************************************************
// Assembly         : DevOpsAssistant
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="IDevOpsUnitOfWorkAggregateActor.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DevOpsAssistant.Infrastructure.Actors;

using Dapr.Actors;

using Hexalith.Infrastructure.DaprRuntime.Handlers;

/// <summary>
/// DevOps unit of work aggregate actor interface <see cref="DevOpsWorkUnitAggretateActor" />.
/// Extends the <see cref="IActor" />.
/// Extends the <see cref="ICommandProcessorActor" />.
/// </summary>
public interface IDevOpsUnitOfWorkAggregateActor : IActor, ICommandProcessorActor
{
}
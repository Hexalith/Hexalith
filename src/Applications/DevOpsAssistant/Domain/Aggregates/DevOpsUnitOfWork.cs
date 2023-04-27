// ***********************************************************************
// Assembly         : DevOpsAssistant
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="DevOpsUnitOfWork.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace DevOpsAssistant.Domain.Aggregates;

using Hexalith.Domain.Abstractions.Aggregates;
using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Class DevOpsUnitOfWork.
/// </summary>
public class DevOpsUnitOfWork : IAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DevOpsUnitOfWork"/> class.
    /// </summary>
    public DevOpsUnitOfWork()
    {
        AggregateId = string.Empty;
        AggregateName = string.Empty;
    }

    /// <inheritdoc/>
    public string AggregateId { get; }

    /// <inheritdoc/>
    public string AggregateName { get; }

    /// <inheritdoc/>
    public IAggregate Apply(BaseEvent domainEvent) => throw new NotImplementedException();
}
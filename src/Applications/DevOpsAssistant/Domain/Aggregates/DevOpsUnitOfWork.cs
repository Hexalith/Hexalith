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

using System.Collections.Generic;

using Hexalith.Domain.Abstractions.Aggregates;
using Hexalith.Domain.Abstractions.Events;

/// <summary>
/// Class DevOpsUnitOfWork.
/// </summary>
public class DevOpsUnitOfWork : IAggregate
{
    public static IAggregate Apply(IEnumerable<BaseEvent> events) => throw new NotImplementedException();

    public IAggregate Apply(BaseEvent domainEvent) => throw new NotImplementedException();
}
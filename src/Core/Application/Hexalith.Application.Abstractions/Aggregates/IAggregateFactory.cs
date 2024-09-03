// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-04-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-04-2024
// ***********************************************************************
// <copyright file="IAggregateFactory.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.Aggregates;

using System;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateFactory.
/// </summary>
public interface IAggregateFactory
{
    /// <summary>
    /// Creates the specified aggregate name.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>IAggregate.</returns>
    IAggregate Create(string aggregateName);

    /// <summary>
    /// Gets the type of the aggregate.
    /// </summary>
    /// <param name="aggregateName">Name of the aggregate.</param>
    /// <returns>Type.</returns>
    Type GetAggregateType(string aggregateName);
}
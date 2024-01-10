// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-10-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="IAggregateProvider - Copy.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateProvider
/// </summary>
public interface IAggregateProvider
{
    /// <summary>
    /// Gets the name of the aggregate.
    /// </summary>
    /// <value>The name of the aggregate.</value>
    string AggregateName { get; }

    /// <summary>
    /// Gets the type of the aggregate.
    /// </summary>
    /// <value>The type of the aggregate.</value>
    Type AggregateType { get; }

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>IAggregate.</returns>
    IAggregate Create();
}
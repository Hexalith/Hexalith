// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 04-25-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-25-2023
// ***********************************************************************
// <copyright file="IAggregateStateManagerFactory.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Application.Abstractions.Aggregates;

using Hexalith.Domain.Abstractions.Aggregates;

/// <summary>
/// Interface IAggregateStateManagerFactory.
/// </summary>
public interface IAggregateStateManagerFactory
{
    /// <summary>
    /// Gets this instance.
    /// </summary>
    /// <typeparam name="TAggregate">The aggregate type.</typeparam>
    /// <returns>IAggregateStateManager&lt;T&gt;.</returns>
    IAggregateStateManager<TAggregate> CreateManager<TAggregate>()
        where TAggregate : IAggregate;
}
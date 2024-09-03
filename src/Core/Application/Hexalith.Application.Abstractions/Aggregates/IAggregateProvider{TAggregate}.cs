// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-10-2024
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-10-2024
// ***********************************************************************
// <copyright file="IAggregateProvider{TAggregate}.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Aggregates;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface IAggregateProvider.
/// </summary>
/// <typeparam name="TAggregate">The type of the t aggregate.</typeparam>
public interface IAggregateProvider<TAggregate> : IAggregateProvider
    where TAggregate : IAggregate, new()
{
    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <returns>TAggregate.</returns>
    new TAggregate Create();
}
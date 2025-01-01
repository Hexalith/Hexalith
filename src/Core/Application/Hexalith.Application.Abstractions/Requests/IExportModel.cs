// <copyright file="IExportModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Interface for exporting a model from a domain aggregate.
/// </summary>
public interface IExportModel
{
    /// <summary>
    /// Creates an export model from the specified domain aggregate.
    /// </summary>
    /// <param name="aggregate">The domain aggregate.</param>
    /// <returns>An instance of <see cref="IExportModel"/>.</returns>
    static abstract IExportModel CreateExportModel(IDomainAggregate aggregate);
}
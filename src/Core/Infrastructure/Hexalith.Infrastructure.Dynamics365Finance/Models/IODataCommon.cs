// <copyright file="IODataElement - Copy.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Interface for Dynamics 365 for finance and operations OData elements.
/// </summary>
public interface IODataCommon
{
    /// <summary>
    /// Gets the record Etag for concurrency checks.
    /// </summary>
    string? Etag { get; }

    /// <summary>
    /// Gets the entity name.
    /// </summary>
    /// <returns>The name.</returns>
    static abstract string EntityName();
}
// <copyright file="IPrimaryPerCompanyKey.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

/// <summary>
/// Entity per company key interface.
/// </summary>
public interface IPrimaryPerCompanyKey
{
    /// <summary>
    /// Gets the data area identifier.
    /// </summary>
    /// <value>The data area identifier.</value>
    string? DataAreaId { get; }
}

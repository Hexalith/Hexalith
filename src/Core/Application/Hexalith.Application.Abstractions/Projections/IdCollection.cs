// <copyright file="IdCollection.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Projections;

using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Represents a collection of identifiers.
/// </summary>
/// <param name="NextPageId">The next page identifier.</param>
/// <param name="Ids">The identifiers.</param>
[DataContract]
public record IdCollection([property: DataMember(Order = 1)] int? NextPageId, [property: DataMember(Order = 2)] IEnumerable<string> Ids)
{
    /// <summary>
    /// Gets the collection identifier based on the aggregate name and partition identifier.
    /// </summary>
    /// <param name="partitionId">The partition identifier.</param>
    /// <param name="aggregateName">The name of the aggregate.</param>
    /// <returns>A string representing the collection identifier.</returns>
    public static string GetCollectionId(string partitionId, string aggregateName)
        => $"{partitionId}-{aggregateName}";
}
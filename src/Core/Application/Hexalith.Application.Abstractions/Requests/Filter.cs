// <copyright file="Filter.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Represents a filter used in requests.
/// </summary>
/// <param name="Order">The order of the filter.</param>
/// <param name="Group">The group of the filter.</param>
/// <param name="FieldName">The field name to filter on.</param>
/// <param name="Operation">The filter operation.</param>
/// <param name="Value">The value to filter by.</param>
/// <param name="And">Indicates if the filter is combined with AND logic.</param>
public record Filter(string FieldName, FilterOperation Operation, string? Value, bool And, int Order, string? Group)
{
    /// <summary>
    /// Creates a filter with the Equal operation.
    /// </summary>
    /// <param name="fieldName">The field name to filter on.</param>
    /// <param name="value">The value to filter by.</param>
    /// <returns>A new filter with the Equal operation.</returns>
    public static Filter Equal(string fieldName, string value)
        => new(fieldName, FilterOperation.Equal, value, false, 0, null);
}
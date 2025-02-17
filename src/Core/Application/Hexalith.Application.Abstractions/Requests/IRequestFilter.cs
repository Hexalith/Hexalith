// <copyright file="IRequestFilter.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Defines a filter for requests.
/// </summary>
public interface IRequestFilter
{
    /// <summary>
    /// Determines whether the specified value complies with the filter.
    /// </summary>
    /// <param name="value">The value to check against the filter.</param>
    /// <returns><c>true</c> if the value complies with the filter; otherwise, <c>false</c>.</returns>
    bool CompliesToFilter(object? value);
}
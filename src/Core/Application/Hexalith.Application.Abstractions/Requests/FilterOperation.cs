// <copyright file="FilterOperation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

/// <summary>
/// Specifies the filter operations that can be applied to a query.
/// </summary>
public enum FilterOperation
{
    /// <summary>
    /// The equal operation.
    /// </summary>
    Equal,

    /// <summary>
    /// The not equal operation.
    /// </summary>
    NotEqual,

    /// <summary>
    /// The greater than operation.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// The greater than or equal operation.
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// The less than operation.
    /// </summary>
    LessThan,

    /// <summary>
    /// The less than or equal operation.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// The contains operation.
    /// </summary>
    Contains,

    /// <summary>
    /// The starts with operation.
    /// </summary>
    StartsWith,

    /// <summary>
    /// The ends with operation.
    /// </summary>
    EndsWith,
}
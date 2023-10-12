// ***********************************************************************
// Assembly         : Hexalith.Extensions
// Author           : Jérôme Piquot
// Created          : 10-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-12-2023
// ***********************************************************************
// <copyright file="ApplicationError.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Common;

/// <summary>
/// Event deserialization error.
/// Implements the <see cref="ErrorCategory" />.
/// </summary>
/// <seealso cref="ErrorCategory" />
public record ApplicationError
{
    /// <summary>
    /// Gets a the explanation arguments.
    /// </summary>
    /// <value>The arguments.</value>
    public IEnumerable<object>? Arguments { get; init; }

    /// <summary>
    /// Gets a the technical explanation arguments.
    /// </summary>
    /// <value>The technical arguments.</value>
    public IEnumerable<object>? TechnicalArguments { get; init; }

    /// <summary>
    /// Gets a human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    /// <value>The detail.</value>
    public string? Detail { get; init; }

    /// <summary>
    /// Gets more information on the technical reason on the error.
    /// </summary>
    /// <value>The technical detail.</value>
    public string? TechnicalDetail { get; init; }

    /// <summary>
    /// Gets the inner error.
    /// </summary>
    /// <value>The inner error.</value>
    public ApplicationError? InnerError { get; init; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    /// <value>The type.</value>
    public string? Type { get; init; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    /// <value>The category.</value>
    public ErrorCategory Category { get; init; }

    /// <summary>
    /// Gets error type.
    /// </summary>
    /// <value>The title.</value>
    public string? Title { get; init; }
}
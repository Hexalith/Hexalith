// <copyright file="Error.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;
/// <summary>
/// Event deserialization error.
/// Implements the <see cref="ErrorCategory" />.
/// </summary>
/// <seealso cref="ErrorCategory" />
public record Error
{
    /// <summary>
    /// Gets a the explanation arguments.
    /// </summary>
    public object?[] Arguments { get; init; } = Array.Empty<object>();

    /// <summary>
    /// Gets a the technical explanation arguments.
    /// </summary>
    public object?[] TechnicalArguments { get; init; } = Array.Empty<object>();

    /// <summary>
    /// Gets a human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    public string? Detail { get; init; }

    /// <summary>
    /// Gets more information on the technical reason on the error.
    /// </summary>
    public string? TechnicalDetail { get; init; }

    /// <summary>
    /// Gets the inner error.
    /// </summary>
    public Error? InnerError { get; init; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public string? Type { get; init; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    public ErrorCategory Category { get; init; }

    /// <summary>
    /// Gets error type.
    /// </summary>
    /// <value>The title.</value>
    public string? Title { get; init; }
}
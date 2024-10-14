// <copyright file="ApplicationError.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

using System;
using System.Runtime.Serialization;

using Hexalith.Extensions.Helpers;

/// <summary>
/// Event deserialization error.
/// Implements the <see cref="ErrorCategory" />.
/// </summary>
/// <seealso cref="ErrorCategory" />
[DataContract]
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
    [DataMember]
    public string? Detail { get; init; }

    /// <summary>
    /// Gets more information on the technical reason on the error.
    /// </summary>
    /// <value>The technical detail.</value>
    [DataMember]
    public string? TechnicalDetail { get; init; }

    /// <summary>
    /// Gets the inner error.
    /// </summary>
    /// <value>The inner error.</value>
    [DataMember]
    public ApplicationError? InnerError { get; init; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    /// <value>The type.</value>
    [DataMember]
    public string? Type { get; init; }

    /// <summary>
    /// Gets the error type.
    /// </summary>
    /// <value>The category.</value>
    [DataMember]
    public ErrorCategory Category { get; init; }

    /// <summary>
    /// Gets error type.
    /// </summary>
    /// <value>The title.</value>
    [DataMember]
    public string? Title { get; init; }

    /// <summary>
    /// Gets the detail message.
    /// </summary>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>System.String.</returns>
    public string GetDetailMessage(IFormatProvider formatProvider)
        => string.IsNullOrWhiteSpace(Detail) ? string.Empty : StringHelper.FormatWithNamedPlaceholders(formatProvider, Detail, Arguments);

    /// <summary>
    /// Gets the technical message.
    /// </summary>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>System.Nullable&lt;System.String&gt;.</returns>
    public string? GetTechnicalMessage(IFormatProvider formatProvider)
        => string.IsNullOrWhiteSpace(TechnicalDetail) ? string.Empty : StringHelper.FormatWithNamedPlaceholders(formatProvider, TechnicalDetail, TechnicalArguments);
}
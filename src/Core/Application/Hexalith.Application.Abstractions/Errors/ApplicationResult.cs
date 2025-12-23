// <copyright file="ApplicationResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Errors;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Commons.Errors;

/// <summary>
/// Class ApplicationResult.
/// </summary>
/// <typeparam name="T">Type of the value.</typeparam>
public class ApplicationResult<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationResult{T}"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public ApplicationResult(T value)
    {
        Value = value;
        Error = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationResult{T}"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    public ApplicationResult(ApplicationError error)
    {
        Error = error;
        Value = default!;
    }

    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>The error.</value>
    public ApplicationError? Error { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance has failed.
    /// </summary>
    /// <value><c>true</c> if this instance has failed; otherwise, <c>false</c>.</value>
    [IgnoreDataMember]
    [JsonIgnore]
    public bool HasFailed => Error != null;

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    /// <exception cref="InvalidOperationException">The result has failed.</exception>
    public T Value => HasFailed ? throw new InvalidOperationException("The result has failed.") : field;
}
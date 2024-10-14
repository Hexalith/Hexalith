// <copyright file="ValueOrError.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

/// <summary>
/// Conditional value class.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class ValueOrError<T>
{
    /// <summary>
    /// The error.
    /// </summary>
    private readonly ApplicationError? _error;

    /// <summary>
    /// The value.
    /// </summary>
    private readonly T _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueOrError{T}" /> class.
    /// Initializes a new valued instance of the <see cref="ValueOrError{T}" /> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public ValueOrError(T value)
    {
        HasValue = true;
        _value = value;
        _error = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueOrError{T}"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="error">The error.</param>
    /// <param name="hasValue">if set to <c>true</c> [has value].</param>
    private ValueOrError(T value, ApplicationError error, bool hasValue)
    {
        HasValue = hasValue;
        _value = value;
        _error = error;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is success.
    /// </summary>
    /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
    /// <exception cref="System.InvalidOperationException">No error.</exception>
    public ApplicationError Error => (HasValue || _error == null) ? throw new InvalidOperationException("No error") : _error;

    /// <summary>
    /// Gets a value indicating whether this instance is success.
    /// </summary>
    /// <value><c>true</c> if this instance is success; otherwise, <c>false</c>.</value>
    public bool HasValue { get; }

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    /// <exception cref="System.InvalidOperationException">No value.</exception>
    public T Value => HasValue ? _value : throw new InvalidOperationException("No value");

    /// <summary>
    /// Withes the error.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>ValueOrError&lt;T&gt;.</returns>
#pragma warning disable CA1000 // Do not declare static members on generic types

    public static ValueOrError<T> WithError(ApplicationError error)
#pragma warning restore CA1000 // Do not declare static members on generic types
        => new(default!, error, false);
}
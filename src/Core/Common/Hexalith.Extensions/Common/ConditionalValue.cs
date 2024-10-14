// <copyright file="ConditionalValue.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

/// <summary>
/// Conditional value class.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public class ConditionalValue<T>
{
    /// <summary>
    /// The value.
    /// </summary>
    private readonly T _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalValue{T}"/> class.
    /// Initializes a new valued instance of the <see cref="ConditionalValue{T}" /> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public ConditionalValue(T value)
    {
        HasValue = true;
        _value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalValue{T}"/> class.
    /// Initializes a new instance without a value of the <see cref="ConditionalValue{T}"/> class.
    /// </summary>
    public ConditionalValue()
    {
        HasValue = false;
        _value = default!;
    }

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
}
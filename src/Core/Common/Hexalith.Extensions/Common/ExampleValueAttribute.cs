// <copyright file="ExampleValueAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

using System;

/// <summary>
/// Example value attribute used to create a example value for a property.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExampleValueAttribute"/> class.
/// </remarks>
/// <param name="value">The value.</param>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ExampleValueAttribute(object value) : Attribute
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public object Value { get; private set; } = value;
}
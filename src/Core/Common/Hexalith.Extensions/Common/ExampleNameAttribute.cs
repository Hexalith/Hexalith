// <copyright file="ExampleNameAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

using System;

/// <summary>
/// Example value attribute used to create a example value for a property.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ExampleNameAttribute" /> class.
/// </remarks>
/// <param name="name">The name.</param>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ExampleNameAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Name { get; private set; } = name;
}
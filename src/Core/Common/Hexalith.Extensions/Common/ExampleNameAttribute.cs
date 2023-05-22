// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-22-2023
// ***********************************************************************
// <copyright file="ExampleNameAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Extensions.Common;

using System;

/// <summary>
/// Example value attribute used to create a example value for a property.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ExampleNameAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleNameAttribute" /> class.
    /// </summary>
    /// <param name="name">The name.</param>
    public ExampleNameAttribute(string name) => Name = name;

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Name { get; private set; }
}
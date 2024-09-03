// <copyright file="JsonPolymorphicBaseTypeAttribute.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Serialization;

using System;

/// <summary>
/// Attribute to mark a class as a polymorphic type.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JsonPolymorphicDerivedTypeAttribute"/> class.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class JsonPolymorphicBaseTypeAttribute : Attribute
{
}
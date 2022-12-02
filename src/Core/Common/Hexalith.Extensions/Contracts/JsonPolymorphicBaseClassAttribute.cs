// <copyright file="JsonPolymorphicBaseClassAttribute.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Contracts;

using System;

#pragma warning disable S1104 // Fields should not have public accessibility
#pragma warning disable SA1401 // Fields should be private

/// <summary>
/// Attribute to indicate the polymorphic class name and polymorphic property name (default is "$type").
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class JsonPolymorphicBaseClassAttribute : Attribute
{
	/// <summary>
	/// Gets the default polymorphic property name (default is "$type").
	/// </summary>
	public const string DefaultDiscriminatorName = "$type";

	/// <summary>
	/// Discriminator property name in the serialized object. If not set, the default polymorphic property name is used ("$type").
	/// </summary>
	public string? DiscriminatorName;

	/// <summary>
	/// The name of the object used to set the property of the discriminator property. If not defined, the class type name will be used.
	/// </summary>
	public string? Name;
}
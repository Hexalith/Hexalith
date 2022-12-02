// <copyright file="TypeNamePolymorphismOptions.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization;

using Hexalith.Extensions.Contracts;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Options for polymorphic serialization of a base type.
/// </summary>
/// <typeparam name="T">The base type of the hierarchy.</typeparam>
public class TypeNamePolymorphismOptions : JsonPolymorphismOptions
{
	private static List<Type>? _jsonPolymorphicBaseClass;
	private List<JsonDerivedType>? _derivedJsonTypes;

	/// <summary>
	/// Initializes a new instance of the <see cref="TypeNamePolymorphismOptions"/> class.
	/// </summary>
	/// <param name="baseType">The type of the base class.</param>
	/// <exception cref="InvalidOperationException">The base type must be a non generic class.</exception>
	public TypeNamePolymorphismOptions(Type baseType)
	{
		if (baseType == null || baseType.IsGenericType || !baseType.IsClass)
		{
			throw new InvalidOperationException("The base type must be a non generic class.");
		}

		BaseType = baseType;
		TypeDiscriminatorPropertyName = "$type";
		IgnoreUnrecognizedTypeDiscriminators = true;
		UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization;
		foreach (JsonDerivedType t in DerivedJsonTypes)
		{
			DerivedTypes.Add(t);
		}

		if (DerivedTypes.Count < 1)
		{
			throw new InvalidOperationException($"No derived type found for base type {baseType.Name}.");
		}
	}

	/// <summary>
	/// Gets the base type of the hierarchy.
	/// </summary>
	public Type BaseType { get; }

	private static List<Type> JsonPolymorphicBaseClasses => _jsonPolymorphicBaseClass ??= GetJsonPolymorphicBaseClasses();

	private IList<JsonDerivedType> DerivedJsonTypes => _derivedJsonTypes ??= GetJsonTypes();

	/// <summary>
	/// Creates a <see cref="JsonPolymorphismOptions"/> instance for the specified type if one of it's parent classes is decorated with <see cref="JsonPolymorphicBaseClassAttribute"/> and has a concrete child.
	/// </summary>
	/// <param name="type">The type of the class.</param>
	/// <returns>The polymorphism options.</returns>
	public static TypeNamePolymorphismOptions? Create(Type type)
	{
		return JsonPolymorphicBaseClasses.Any(p => p.IsSubclassOf(type) && !p.IsAbstract) ? new TypeNamePolymorphismOptions(type) : null;
	}

	private static List<Type> GetJsonPolymorphicBaseClasses()
	{
		return AppDomain
				.CurrentDomain
				.GetAssemblies()
					.SelectMany(assembly => assembly.GetTypes())
					.Where(IsPolymorphicSerializable)
			.ToList();

		static bool IsPolymorphicSerializable(Type type)
		{
			return type.IsClass && type.GetCustomAttribute<JsonPolymorphicBaseClassAttribute>(inherit: true) != null;
		}
	}

	private List<JsonDerivedType> GetJsonTypes()
	{
		return JsonPolymorphicBaseClasses
					.Where(IsConcreteBaseClass)
					.Select(p => new JsonDerivedType(p, p.Name))
					.ToList();

		bool IsConcreteBaseClass(Type type)
		{
			return !type.IsAbstract && type.IsClass && type.IsSubclassOf(BaseType);
		}
	}
}
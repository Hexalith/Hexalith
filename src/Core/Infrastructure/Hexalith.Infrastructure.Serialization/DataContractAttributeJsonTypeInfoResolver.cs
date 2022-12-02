// <copyright file="DataContractAttributeJsonTypeInfoResolver.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Serialization;

using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Default type resolver for <see cref="JsonSerializer"/>. Inherits from base class <see cref="DefaultJsonTypeInfoResolver"/>.
/// </summary>
public class DataContractAttributeJsonTypeInfoResolver : DefaultJsonTypeInfoResolver
{
	/// <inheritdoc/>
	public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

		if (jsonTypeInfo.Kind == JsonTypeInfoKind.Object &&
			type.GetCustomAttribute<DataContractAttribute>() is not null)
		{
			jsonTypeInfo.Properties.Clear();

			foreach ((PropertyInfo propertyInfo, DataMemberAttribute? attr) in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Select((prop) => (prop, prop.GetCustomAttribute<DataMemberAttribute>()))
				.Where((x) => x.Item2 != null)
				.OrderBy((x) => x.Item2!.Order))
			{
				JsonPropertyInfo jsonPropertyInfo = jsonTypeInfo.CreateJsonPropertyInfo(propertyInfo.PropertyType, attr.Name ?? propertyInfo.Name);
				jsonPropertyInfo.Get =
					propertyInfo.CanRead
					? propertyInfo.GetValue
					: null;

				jsonPropertyInfo.Set = propertyInfo.CanWrite
					? propertyInfo.SetValue
					: null;

				jsonTypeInfo.Properties.Add(jsonPropertyInfo);
			}
		}

		return jsonTypeInfo;
	}
}
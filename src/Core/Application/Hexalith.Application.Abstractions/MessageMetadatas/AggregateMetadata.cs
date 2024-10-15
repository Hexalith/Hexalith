// <copyright file="AggregateMetadata.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Represents the metadata of an aggregate.
/// </summary>
/// <param name="Id">The identifier of the aggregate.</param>
/// <param name="Name">The name of the aggregate.</param>
[DataContract]
public record AggregateMetadata(
    [property:DataMember(Order = 1)]
    [property:JsonPropertyOrder(1)]
    string Id,
    [property:DataMember(Order = 2)]
    [property:JsonPropertyOrder(2)]
    string Name)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateMetadata"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public AggregateMetadata()
        : this(string.Empty, string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateMetadata"/> class.
    /// </summary>
    /// <param name="instance">The aggregate related object (Aggregates, Events, Commands). The object must have the AggregateId and AggragateName properties defined and valued.</param>
    public AggregateMetadata(object instance)
        : this(string.Empty, string.Empty)
    {
        AggregateMetadata meta = Create(instance);
        Id = meta.Id;
        Name = meta.Name;
    }

    /// <summary>
    /// Creates an instance of <see cref="AggregateMetadata"/> from the specified aggregate related object.
    /// </summary>
    /// <param name="instance">The aggregate related object (Aggregates, Events, Commands).</param>
    /// <returns>An instance of <see cref="AggregateMetadata"/>.</returns>
    public static AggregateMetadata Create([NotNull] object instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        // Get the type of the instance object
        Type type = instance.GetType();

        // Function to get property value (instance or static, including base classes)
        string? GetPropertyValue(string propertyName)
        {
            // Check for instance property
            PropertyInfo? instanceProperty = type.GetProperty(propertyName);
            if (instanceProperty != null)
            {
                return instanceProperty.GetValue(instance)?.ToString();
            }

            // Check for static property (including base classes)
            Type? currentType = type;
            while (currentType != null)
            {
                PropertyInfo? staticProperty = currentType.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (staticProperty != null)
                {
                    return staticProperty.GetValue(null)?.ToString();
                }

                currentType = currentType.BaseType;
            }

            return null;
        }

        // Get the values of the properties
        string? id = GetPropertyValue(nameof(IDomainAggregate.AggregateId));
        string? name = GetPropertyValue(nameof(IDomainAggregate.AggregateName));

        return id == null || name == null
            ? throw new InvalidOperationException($"Invalid aggregate instance: the {nameof(IDomainAggregate.AggregateName)} or {nameof(IDomainAggregate.AggregateId)} properties are missing or undefined. {JsonSerializer.Serialize(instance)}")
            : new AggregateMetadata(id, name);
    }
}
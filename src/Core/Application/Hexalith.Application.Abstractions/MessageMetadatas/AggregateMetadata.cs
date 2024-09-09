// <copyright file="AggregateMetadata.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.MessageMetadatas;

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Domain.Aggregates;

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

        // Use reflection to get the Id and Name properties
        PropertyInfo? idProperty = type.GetProperty(nameof(IDomainAggregate.AggregateId));
        PropertyInfo? nameProperty = type.GetProperty(nameof(IDomainAggregate.AggregateName));

        // Get the values of the properties from the instance object
        string? id = idProperty?.GetValue(instance)?.ToString();
        string? name = nameProperty?.GetValue(instance)?.ToString();

        return id == null || name == null
            ? throw new InvalidOperationException($"Invalid aggregate instance the {nameof(IDomainAggregate.AggregateName)} or {nameof(IDomainAggregate.AggregateId)} properties are missing or undefined. {JsonSerializer.Serialize(instance)}")
            : new AggregateMetadata(id, name);
    }
}
// <copyright file="JsonStreamItem.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.StreamStores;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Persited stream item.
/// </summary>
[DataContract]
public class JsonStreamItem : IStreamItem
{
    private IDataFragment? _message;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonStreamItem" /> class.
    /// </summary>
    /// <param name="sequence">The stream sequence number.</param>
    /// <param name="dataType">The data type fully qualified name.</param>
    /// <param name="data">Json serialized data object string.</param>
    /// <param name="metaType">The metadata type fully qualified name.</param>
    /// <param name="metadata">Json serialized metadata object string.</param>
    [JsonConstructor]
    public JsonStreamItem(long sequence, string dataType, string data, string metaType, string metadata)
    {
        Sequence = sequence;
        DataType = dataType;
        Data = data;
        MetaType = metaType;
        Metadata = metadata;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonStreamItem" /> class. Initializer for serializers that require a parameterless constructor.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Info Code Smell", "S1133:Deprecated code should be removed", Justification = "Used for serialization")]
    public JsonStreamItem() => Data = Metadata = DataType = MetaType = string.Empty;

    /// <summary>
    /// Gets the serialized data object.
    /// </summary>
    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public string Data { get; private set; }

    /// <summary>
    /// Gets the data type fully qualified name.
    /// </summary>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string DataType { get; private set; }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public IDataFragment Message => _message ??= CreateDataFragment();

    /// <summary>
    /// Gets the serialized meta data object.
    /// </summary>
    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public string Metadata { get; private set; }

    /// <summary>
    /// Gets the metadata type fully qualified name.
    /// </summary>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string MetaType { get; private set; }

    /// <summary>
    /// Gets the stream sequence number.
    /// </summary>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public long Sequence { get; private set; }

    /// <summary>
    /// Create a new stream item from an object instance.
    /// </summary>
    /// <param name="sequence">The stream item sequence number.</param>
    /// <param name="data">The data object.</param>
    /// <param name="options">The Json serializer options.</param>
    /// <returns>A new stream item object.</returns>
    public static IStreamItem CreateStreamItem(
        long sequence,
        IDataFragment data,
        JsonSerializerOptions? options)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new JsonStreamItem(
            sequence,
            data.Data.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(data.Data, options),
            data.Metadata.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(data.Metadata, options));
    }

    /// <summary>
    /// Create a new stream item from an object instance.
    /// </summary>
    /// <param name="sequence">The stream item sequence number.</param>
    /// <param name="data">The data object.</param>
    /// <returns>A new stream item object.</returns>
    public static IStreamItem CreateStreamItem(
        long sequence,
        IDataFragment data)
    {
        ArgumentNullException.ThrowIfNull(data);
        return new JsonStreamItem(
            sequence,
            data.Data.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(data.Data),
            data.Metadata.GetType().AssemblyQualifiedName!,
            JsonSerializer.Serialize(data.Metadata));
    }

    private DataFragment CreateDataFragment()
    {
        Type dataType = Type.GetType(DataType) ?? throw new InvalidOperationException($"Type {DataType} not found.");
        Type metaType = Type.GetType(MetaType) ?? throw new InvalidOperationException($"Type {MetaType} not found.");
        return new DataFragment(
            JsonSerializer.Deserialize(Data, dataType) ?? throw new InvalidOperationException($"Unable to deserialize {Data} to {dataType.FullName}."),
            JsonSerializer.Deserialize(Metadata, metaType) ?? throw new InvalidOperationException($"Unable to deserialize {Metadata} to {metaType.FullName}."));
    }
}
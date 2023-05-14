// <copyright file="DataFragment.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.StreamStores;

/// <summary>
/// Data fragment class.
/// </summary>
[DataContract]
public class DataFragment : IDataFragment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataFragment" /> class.
    /// </summary>
    /// <param name="data">The data object.</param>
    /// <param name="metadata">The metadata object.</param>
    [JsonConstructor]
    public DataFragment(object data, object metadata)
    {
        Data = data;
        Metadata = metadata;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataFragment"/> class.
    /// Initializer for serializers that require a parameterless constructor.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
    public DataFragment()
    {
        Data = Metadata = string.Empty;
    }

    /// <summary>
    /// Gets the data object.
    /// </summary>
    public object Data { get; private set; }

    /// <summary>
    /// Gets the meta data object.
    /// </summary>
    public object Metadata { get; private set; }
}
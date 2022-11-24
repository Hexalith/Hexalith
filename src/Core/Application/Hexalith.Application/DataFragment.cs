// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Application;

using Hexalith.Application.Abstractions;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Data fragment class
/// </summary>
[DataContract]
public class DataFragment : IDataFragment
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DataFragment" /> class.
	/// </summary>
	/// <param name="data">The data object</param>
	/// <param name="metadata">The metadata object</param>
	[JsonConstructor]
	public DataFragment(object data, object metadata)
	{
		Data = data;
		Metadata = metadata;
	}

	/// <summary>
	/// Initializer for serializers that require a parameterless constructor
	/// </summary>
	[Obsolete("This constructor is only for serialization purposes.", true)]
	public DataFragment()
		=> Data = Metadata = string.Empty;

	/// <summary>
	/// Gets the data object
	/// </summary>
	public object Data { get; private set; }

	/// <summary>
	/// Gets the meta data object
	/// </summary>
	public object Metadata { get; private set; }
}
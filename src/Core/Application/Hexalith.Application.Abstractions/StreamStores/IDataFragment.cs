// <copyright file="IDataFragment.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.StreamStores;

/// <summary>
/// Persisted stream item interface.
/// </summary>
public interface IDataFragment
{
	/// <summary>
	/// Gets the data object.
	/// </summary>
	public object Data { get; }

	/// <summary>
	/// Gets the meta data object.
	/// </summary>
	public object Metadata { get; }
}

/// <summary>
/// Persisted stream item interface.
/// </summary>
public interface IDataFragment<out TData, out TMeta> : IDataFragment
{
	/// <summary>
	/// Gets the data object.
	/// </summary>
	public new TData Data { get; }

	/// <summary>
	/// Gets the meta data object.
	/// </summary>
	public new TMeta Metadata { get; }
}
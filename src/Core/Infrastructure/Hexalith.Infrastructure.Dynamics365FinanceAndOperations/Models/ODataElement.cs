// <copyright file="ODataElement.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations entity base class.
/// </summary>
public abstract record ODataElement
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ODataElement"/> class.
	/// </summary>
	/// <param name="etag">The Etag.</param>
	/// <param name="dataAreaId">The company identifier.</param>
	[JsonConstructor]
	protected ODataElement(string etag, string dataAreaId)
	{
		Etag = etag;
		DataAreaId = dataAreaId;
	}

	/// <summary>
	/// Gets the ETag (or entity tag) HTTP response header is an identifier for a specific version of a resource.
	/// Etags help to prevent simultaneous updates of a resource from overwriting each other.
	/// </summary>
	[JsonPropertyName("@odata.etag")]
	public string Etag { get; }

	/// <summary>
	/// Gets the company identifier.
	/// </summary>
	[JsonPropertyName("dataAreaId")]
	public string DataAreaId { get; }
}
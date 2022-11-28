// <copyright file="ODataElement.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Text.Json.Serialization;

public abstract record ODataElement
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ODataElement"/> class.
	/// </summary>
	/// <param name="etag"></param>
	/// <param name="dataAreaId"></param>
	[JsonConstructor]
	protected ODataElement(string etag, string dataAreaId)
	{
		Etag = etag;
		DataAreaId = dataAreaId;
	}

	[JsonPropertyName("@odata.etag")]
	public string Etag { get; }

	[JsonPropertyName("dataAreaId")]
	public string DataAreaId { get; }
}
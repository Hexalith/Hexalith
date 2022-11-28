// <copyright file="ODataResponse.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Runtime.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations entity response content.
/// </summary>
/// <typeparam name="T">Type of the entity object.</typeparam>
[DataContract]
public class ODataResponse<T>
{
	[DataMember(Name = "@odata.context")]
	public string? Context { get; set; }

	[DataMember(Name = "message")]
	public string? Message { get; set; }

	[DataMember(Name = "value")]
	public T? Value { get; set; }
}
// <copyright file="ODataElement.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations entity base class.
/// </summary>
[DataContract]
public record ODataElement(
    string? Etag,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyName("dataAreaId")]
    string DataAreaId) : ODataCommon(Etag)
{
}
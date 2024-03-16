// <copyright file="PerCompanyFilter.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations per company entity filter base class.
/// </summary>
[DataContract]
public record PerCompanyFilter : IPerCompanyFilter
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PerCompanyFilter"/> class.
    /// </summary>
    /// <param name="dataAreaId">The company identifier.</param>
    [JsonConstructor]
    protected PerCompanyFilter(string? dataAreaId) => DataAreaId = dataAreaId;

    /// <summary>
    /// Gets the company.
    /// </summary>
    [JsonPropertyName("dataAreaId")]
    [DataMember]
    public string? DataAreaId { get; }
}
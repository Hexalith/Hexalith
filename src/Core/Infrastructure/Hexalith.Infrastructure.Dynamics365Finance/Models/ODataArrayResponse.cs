// <copyright file="ODataArrayResponse.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations entity list response content.
/// </summary>
/// <typeparam name="T">Type of the entity object.</typeparam>
[DataContract]
public class ODataArrayResponse<T>
{
    /// <summary>
    /// Gets or sets the OData context.
    /// </summary>
    /// <value>
    /// The OData context.
    /// </value>
    [DataMember(Name = "@odata.context")]
    [JsonPropertyName("@odata.context")]
    public string? Context { get; set; }

    /// <summary>
    /// Gets or sets the message if the request fails.
    /// </summary>
    /// <value>
    /// The message if the request fails.
    /// </value>
    [DataMember(Name = "message")]
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the list of entity objects.
    /// </summary>
    /// <value>
    /// The list of entity objects.
    /// </value>
    [DataMember(Name = "value")]
    [JsonPropertyName("value")]
    public ICollection<T> Values { get; set; } = Array.Empty<T>();
}
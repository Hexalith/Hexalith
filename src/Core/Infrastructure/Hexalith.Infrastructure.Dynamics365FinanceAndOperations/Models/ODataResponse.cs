// <copyright file="ODataResponse.cs" company="Fiveforty SAS Paris France">
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

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

using System.Runtime.Serialization;

/// <summary>
/// Dynamics 365 Finance and Operations entity response content.
/// </summary>
/// <typeparam name="T">Type of the entity object.</typeparam>
[DataContract]
public class ODataResponse<T>
{
    /// <summary>
    /// Gets or sets the OData context.
    /// </summary>
    /// <value>
    /// The OData context.
    /// </value>
    [DataMember(Name = "@odata.context")]
    public string? Context { get; set; }

    /// <summary>
    /// Gets or sets message when there is an error.
    /// </summary>
    /// <value>
    /// Message when there is an error.
    /// </value>
    [DataMember(Name = "message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the entity value.
    /// </summary>
    /// <value>
    /// The entity value.
    /// </value>
    [DataMember(Name = "value")]
    public T? Value { get; set; }
}
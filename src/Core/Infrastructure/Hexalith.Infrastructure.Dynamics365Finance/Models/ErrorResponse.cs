// <copyright file="ErrorResponse.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class ErrorResponse.
/// </summary>
[DataContract]
public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error.
    /// </summary>
    /// <value>The error.</value>
    [JsonPropertyName("error")]
    public ErrorMessage? Error { get; set; }
}
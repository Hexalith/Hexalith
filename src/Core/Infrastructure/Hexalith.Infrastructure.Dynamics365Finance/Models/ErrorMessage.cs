// <copyright file="ErrorMessage.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class ErrorMessage.
/// </summary>
[DataContract]
public class ErrorMessage
{
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The code.</value>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the inner error.
    /// </summary>
    /// <value>The inner error.</value>
    [JsonPropertyName("innererror")]
    public InnerErrorMessage? InnerError { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}
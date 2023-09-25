// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 03-07-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-07-2023
// ***********************************************************************
// <copyright file="InnerErrorMessage.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class ErrorMessage.
/// </summary>
[DataContract]
public class InnerErrorMessage
{
    /// <summary>
    /// Gets or sets the internal exception.
    /// </summary>
    /// <value>The internal exception.</value>
    [JsonPropertyName("internalexception")]
    public InnerErrorMessage? InternalException { get; set; }

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets the stack trace.
    /// </summary>
    /// <value>The stack trace.</value>
    [JsonPropertyName("stacktrace")]
    public string? StackTrace { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}
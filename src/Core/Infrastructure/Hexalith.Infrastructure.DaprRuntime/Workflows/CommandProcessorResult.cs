// <copyright file="CommandProcessorResult.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Workflows;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Represents the result of a command processor.
/// </summary>
/// <remarks>
/// This class contains information about the last processed command, the next retry time, and the number of retries.
/// </remarks>
[DataContract]
public sealed record CommandProcessorResult(
    [property: DataMember(Order = 1)]
    [property: JsonPropertyOrder(1)]
    long LastCommandId,
    [property: DataMember(Order = 2)]
    [property: JsonPropertyOrder(1)]
    long RetryCount,
    [property: DataMember(Order = 3)]
    [property: JsonPropertyOrder(1)]
    DateTimeOffset? NextRetryTime)
{
}
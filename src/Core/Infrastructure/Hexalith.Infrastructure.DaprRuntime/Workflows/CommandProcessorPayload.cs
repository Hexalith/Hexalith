// <copyright file="CommandProcessorPayload.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Workflows;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Tasks;

/// <summary>
/// Represents the payload for a command processor.
/// </summary>
/// <remarks>
/// This class is used to transfer data between the command processor and the workflow.
/// </remarks>
[DataContract]
[Serializable]
public class CommandProcessorPayload
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessorPayload"/> class.
    /// </summary>
    [Obsolete("This constructor is for serialization only.", true)]
    public CommandProcessorPayload()
    {
        AggregateId = AggregateName = string.Empty;
        RetryPolicy = ResiliencyPolicy.None;
        StartedTime = RetryTime = DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessorPayload"/> class.
    /// </summary>
    /// <param name="payload">The payload to copy from.</param>
    /// <param name="retryCount">The retry count.</param>
    public CommandProcessorPayload(CommandProcessorPayload payload, int retryCount)
        : this(
             (payload ?? throw new ArgumentNullException(nameof(payload))).AggregateId,
             payload.AggregateName,
             retryCount,
             payload.RetryPolicy,
             payload.StartedTime)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandProcessorPayload"/> class.
    /// </summary>
    /// <param name="aggregateId">The aggregate ID.</param>
    /// <param name="aggregateName">The aggregate name.</param>
    /// <param name="retryCount">The retry count.</param>
    /// <param name="retryPolicy">The retry policy.</param>
    /// <param name="startedTime">The started time.</param>
    public CommandProcessorPayload(
        string aggregateId,
        string aggregateName,
        int retryCount,
        ResiliencyPolicy retryPolicy,
        DateTimeOffset startedTime)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateId);
        ArgumentException.ThrowIfNullOrWhiteSpace(aggregateName);
        ArgumentNullException.ThrowIfNull(retryPolicy);
        ArgumentNullException.ThrowIfNull(startedTime);

        AggregateId = aggregateId;
        AggregateName = aggregateName;
        RetryCount = retryCount;
        RetryDelay = retryPolicy.EvaluatePeriod(retryCount);
        RetryPolicy = retryPolicy;
        RetryTime = retryPolicy.NextRetryTime(startedTime, retryCount);
        StartedTime = startedTime;
    }

    /// <summary>
    /// Gets or sets the aggregate ID.
    /// </summary>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string AggregateId { get; set; }

    /// <summary>
    /// Gets or sets the aggregate name.
    /// </summary>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string AggregateName { get; set; }

    /// <summary>
    /// Gets or sets the retry count.
    /// </summary>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public int RetryCount { get; set; }

    /// <summary>
    /// Gets or sets the retry delay.
    /// </summary>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public TimeSpan RetryDelay { get; set; }

    /// <summary>
    /// Gets or sets the retry policy.
    /// </summary>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    public ResiliencyPolicy RetryPolicy { get; set; }

    /// <summary>
    /// Gets or sets the retry time.
    /// </summary>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public DateTimeOffset RetryTime { get; set; }

    /// <summary>
    /// Gets or sets the started time.
    /// </summary>
    [DataMember(Order = 7)]
    [JsonPropertyOrder(7)]
    public DateTimeOffset StartedTime { get; set; }
}
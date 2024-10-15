// <copyright file="ResiliencyPolicy.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Retry policy.
/// </summary>
[DataContract]
public class ResiliencyPolicy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResiliencyPolicy" /> class.
    /// </summary>
    /// <param name="maximumRetries">The maximum number of retries.</param>
    /// <param name="initialPeriod">The retry initial period in milliseconds.</param>
    /// <param name="period">The retry period in milliseconds.</param>
    /// <param name="maximumExponentialPeriod">The maximum retry period if exponential.</param>
    /// <param name="timeout">The maximum retry total time.</param>
    /// <param name="exponential">Use exponential periods.</param>
    [JsonConstructor]
    public ResiliencyPolicy(
        int maximumRetries,
        TimeSpan initialPeriod,
        TimeSpan period,
        TimeSpan maximumExponentialPeriod,
        TimeSpan timeout,
        bool exponential)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        MaximumRetries = maximumRetries;
        InitialPeriod = initialPeriod;
        Period = period;
        MaximumExponentialPeriod = maximumExponentialPeriod;
        Timeout = timeout;
        Exponential = exponential;
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResiliencyPolicy" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public ResiliencyPolicy()
    {
        MaximumRetries = 0;
        InitialPeriod = TimeSpan.Zero;
        Period = TimeSpan.Zero;
        MaximumExponentialPeriod = TimeSpan.Zero;
        Timeout = TimeSpan.Zero;
        Exponential = false;
    }

    /// <summary>
    /// Gets the default no retry policy.
    /// </summary>
    /// <value>The none.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public static ResiliencyPolicy None { get; } = new(0, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, exponential: false);

    /// <summary>
    /// Gets or sets a value indicating whether use exponential periods.
    /// </summary>
    /// <value>The exponential.</value>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    public bool Exponential
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the initial retry period.
    /// </summary>
    /// <value>The initial period.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public TimeSpan InitialPeriod
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets a value the maximum exponential period value in milliseconds.
    /// </summary>
    /// <value>The maximum exponential period.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public TimeSpan MaximumExponentialPeriod
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the maximum number of retries.
    /// </summary>
    /// <value>The maximum retries.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public int MaximumRetries
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the retry period in milliseconds.
    /// </summary>
    /// <value>The period.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public TimeSpan Period
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Gets or sets the timeout.
    /// </summary>
    /// <value>The timeout.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public TimeSpan Timeout
    {
        get;
        [Obsolete("Setter used only for serialization purposes.", false)]
        set;
    }

    /// <summary>
    /// Create an exponential retry, starting from one millisecond without any timeout.
    /// </summary>
    /// <returns>The retry policy.</returns>
    public static ResiliencyPolicy CreateDefaultExponentialRetry() => new(-1, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.FromDays(1), TimeSpan.FromDays(30), exponential: true);

    /// <summary>
    /// Create an exponential retry, starting from one millisecond without any timeout.
    /// </summary>
    /// <returns>The retry policy.</returns>
    public static ResiliencyPolicy CreateEternalExponentialRetry() => new(-1, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1), TimeSpan.MaxValue, TimeSpan.MaxValue, exponential: true);

    /// <summary>
    /// Create an linear retry with a defined period and without any global timeout.
    /// </summary>
    /// <param name="period">A fix period of time to wait for between each retry.</param>
    /// <returns>The retry policy.</returns>
    public static ResiliencyPolicy CreateEternalRetry(TimeSpan period) => new(-1, period, period, TimeSpan.Zero, TimeSpan.MaxValue, exponential: false);

    /// <summary>
    /// Gets the retry policy status.
    /// </summary>
    /// <param name="startedDate">The started date.</param>
    /// <param name="retryCount">The number of retries.</param>
    /// <returns>The retry policy status.</returns>
    public RetryStatus CanRetry(DateTimeOffset startedDate, int retryCount)
    {
        return RetryLimitReached(retryCount)
            ? RetryStatus.Stopped
            : TimeoutReached(startedDate)
            ? RetryStatus.Stopped
            : NextRetryTime(startedDate, retryCount) < DateTimeOffset.UtcNow ? RetryStatus.Enabled : RetryStatus.Suspended;
    }

    /// <summary>
    /// Evaluates the period.
    /// </summary>
    /// <param name="retry">The retry.</param>
    /// <returns>System.TimeSpan.</returns>
    public TimeSpan EvaluatePeriod(int retry)
    {
        if (retry <= 1)
        {
            return InitialPeriod;
        }

        // Get new period to wait
        long period = Period.Ticks;
        long maximumPeriod = long.MaxValue;
        if (Exponential)
        {
            if (MaximumExponentialPeriod != TimeSpan.Zero)
            {
                maximumPeriod = MaximumExponentialPeriod.Ticks;
            }

            long multiplier = FibonacciSequence.Number(retry);
            period = maximumPeriod / multiplier <= Period.Ticks ? maximumPeriod : Period.Ticks * multiplier;
        }

        if (period >= maximumPeriod)
        {
            return new TimeSpan(maximumPeriod);
        }

        long previousPeriod = EvaluatePeriod(retry - 1).Ticks;

        // Check for overflow
        if (maximumPeriod - previousPeriod <= period)
        {
            period = maximumPeriod;
        }
        else
        {
            period += previousPeriod;
        }

        return new TimeSpan(period);
    }

    /// <summary>
    /// Gets next retry time.
    /// </summary>
    /// <param name="startedDate">Initial start date and time.</param>
    /// <param name="retryCount">Number of retries.</param>
    /// <returns>The next retry date and time.</returns>
    public DateTimeOffset NextRetryTime(DateTimeOffset startedDate, int retryCount)
        => startedDate.Add(EvaluatePeriod(retryCount));

    /// <summary>
    /// Time to wait until retry can be performed.
    /// </summary>
    /// <param name="startedDate">The started date.</param>
    /// <param name="retryCount">The retry count.</param>
    /// <returns>System.TimeSpan.</returns>
    public TimeSpan RetryWaitTime(DateTimeOffset startedDate, int retryCount)
    {
        DateTimeOffset date = NextRetryTime(startedDate, retryCount);
        TimeSpan waitTime = date.UtcDateTime - DateTime.UtcNow;
        waitTime = (waitTime.Ticks < 0) ? TimeSpan.Zero : waitTime;
        return waitTime;
    }

    /// <summary>
    /// Retries the limit reached.
    /// </summary>
    /// <param name="retryCount">The retry count.</param>
    /// <returns>bool.</returns>
    private bool RetryLimitReached(int retryCount) => retryCount > MaximumRetries && MaximumRetries >= 0;

    /// <summary>
    /// Timeouts the reached.
    /// </summary>
    /// <param name="startedDate">The started date.</param>
    /// <returns>bool.</returns>
    private bool TimeoutReached(DateTimeOffset startedDate) => Timeout != TimeSpan.MaxValue && startedDate.Add(Timeout) < DateTimeOffset.UtcNow;
}
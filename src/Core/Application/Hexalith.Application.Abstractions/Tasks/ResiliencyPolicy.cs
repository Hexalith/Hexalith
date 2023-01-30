// <copyright file="ResiliencyPolicy.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Tasks;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Helpers;

/// <summary>
/// Retry policy.
/// </summary>
[DataContract]
public class ResiliencyPolicy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResiliencyPolicy"/> class.
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
        MaximumRetries = maximumRetries;
        InitialPeriod = initialPeriod;
        Period = period;
        MaximumExponentialPeriod = maximumExponentialPeriod;
        Timeout = timeout;
        Exponential = exponential;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ResiliencyPolicy"/> class.
    /// </summary>
    [Obsolete("This constructor is only for serialization purposes.", true)]
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
    [JsonIgnore]
    [IgnoreDataMember]
    public static ResiliencyPolicy None { get; } = new(0, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, TimeSpan.Zero, exponential: false);

    /// <summary>
    /// Gets a value indicating whether use exponential periods.
    /// </summary>
    public bool Exponential { get; private set; }

    /// <summary>
    /// Gets the initial retry period.
    /// </summary>
    public TimeSpan InitialPeriod { get; }

    /// <summary>
    /// Gets a value the maximum exponential period value in milliseconds.
    /// </summary>
    public TimeSpan MaximumExponentialPeriod { get; private set; }

    /// <summary>
    /// Gets the maximum number of retries.
    /// </summary>
    public int MaximumRetries { get; private set; }

    /// <summary>
    /// Gets the retry period in milliseconds.
    /// </summary>
    public TimeSpan Period { get; private set; }

    /// <summary>
    /// Gets the timeout.
    /// </summary>
    public TimeSpan Timeout { get; private set; }

    /// <summary>
    /// Create an exponential retry, starting from one millisecond without any timeout.
    /// </summary>
    /// <returns>The retry policy.</returns>
    public static ResiliencyPolicy CreateEternalExponentialRetry()
    {
        return new ResiliencyPolicy(-1, TimeSpan.FromMilliseconds(1), TimeSpan.FromMilliseconds(1), TimeSpan.MaxValue, TimeSpan.MaxValue, exponential: true);
    }

    /// <summary>
    /// Create an linear retry with a defined period and without any global timeout.
    /// </summary>
    /// <param name="period">A fix period of time to wait for between each retry.</param>
    /// <returns>The retry policy.</returns>
    public static ResiliencyPolicy CreateEternalRetry(TimeSpan period)
    {
        return new ResiliencyPolicy(-1, period, period, TimeSpan.Zero, TimeSpan.MaxValue, exponential: false);
    }

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
    /// Gets next retry time.
    /// </summary>
    /// <param name="startedDate">Initial start date and time.</param>
    /// <param name="retryCount">Number of retries.</param>
    /// <returns>The next retry date and time.</returns>
    public DateTimeOffset NextRetryTime(DateTimeOffset startedDate, int retryCount)
    {
        return startedDate.Add(EvaluatePeriod(retryCount));
    }

    private TimeSpan EvaluatePeriod(int retry)
    {
        if (retry < 1)
        {
            return InitialPeriod;
        }

        TimeSpan period = Period * (Exponential ? FibonacciSequence.Number(retry) : 1);
        if (Exponential && period > MaximumExponentialPeriod)
        {
            period = MaximumExponentialPeriod;
        }

        return period + EvaluatePeriod(retry - 1);
    }

    private bool RetryLimitReached(int retryCount)
    {
        return retryCount > MaximumRetries && MaximumRetries >= 0;
    }

    private bool TimeoutReached(DateTimeOffset startedDate)
    {
        return Timeout != TimeSpan.MaxValue && startedDate.Add(Timeout) < DateTimeOffset.UtcNow;
    }
}
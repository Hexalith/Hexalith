// <copyright file="ResiliencyPolicyTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Application.Tasks;

using FluentAssertions;

using Hexalith.Application.Abstractions.Tasks;

using System.Text.Json;

public class ResiliencyPolicyTest
{
    [Fact]
    public void Deserialize_serialized_policy_should_return_same_value()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = new(
            100,
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(100),
            TimeSpan.Zero,
            TimeSpan.FromHours(24),
            exponential: false);
        string json = JsonSerializer.Serialize(policy);
        ResiliencyPolicy? deserialized = JsonSerializer.Deserialize<ResiliencyPolicy>(json);
        _ = deserialized.Should().NotBeNull();
        _ = deserialized.Should().BeEquivalentTo(policy);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(1, 10 + 5)]
    [InlineData(2, 10 + 15)]
    [InlineData(3, 20 + 25)]
    [InlineData(4, 30 + 45)]
    [InlineData(5, 50 + 75)]
    [InlineData(6, 80 + 125)]
    [InlineData(7, 130 + 205)]
    [InlineData(8, 210 + 335)]
    [InlineData(9, 340 + 545)]
    [InlineData(10, 550 + 885)]
    [InlineData(11, 890 + 1435)]
    public void Wait_time_for_each_exponential_retry_should_be_expected_value(int sequence, int value)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        ResiliencyPolicy policy = new(
            100,
            TimeSpan.FromMilliseconds(5),
            TimeSpan.FromMilliseconds(10),
            TimeSpan.FromMilliseconds(5000),
            TimeSpan.FromHours(24),
            exponential: true);
        long waitMilliseconds = policy.NextRetryTime(now, sequence).ToUnixTimeMilliseconds() - now.ToUnixTimeMilliseconds();
        _ = waitMilliseconds.Should().Be(value);
    }

    [Theory]
    [InlineData(0, 200)]
    [InlineData(1, 100 + 200)]
    [InlineData(2, 200 + 200)]
    [InlineData(3, 300 + 200)]
    [InlineData(4, 400 + 200)]
    [InlineData(5, 500 + 200)]
    [InlineData(6, 600 + 200)]
    [InlineData(7, 700 + 200)]
    [InlineData(8, 800 + 200)]
    [InlineData(9, 900 + 200)]
    [InlineData(10, 1000 + 200)]
    [InlineData(11, 1100 + 200)]
    public void Wait_time_for_each_linear_retry_should_be_expected_value(int sequence, int value)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        ResiliencyPolicy policy = new(
            100,
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromMilliseconds(100),
            TimeSpan.Zero,
            TimeSpan.FromHours(24),
            exponential: false);
        long waitMilliseconds = policy.NextRetryTime(now, sequence).ToUnixTimeMilliseconds() - now.ToUnixTimeMilliseconds();
        _ = waitMilliseconds.Should().Be(value);
    }
}
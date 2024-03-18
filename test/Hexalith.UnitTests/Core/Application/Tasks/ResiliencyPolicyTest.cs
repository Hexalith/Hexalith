// <copyright file="ResiliencyPolicyTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Tasks;
using Hexalith.Extensions.Configuration;
using Hexalith.TestMocks;

public class ResiliencyPolicyTest
{
    [Fact]
    public void DataContractSerializeSerializeShouldReturnSameValue()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = GetTestPolicy();
        _ = policy.Should().BeDataContractSerializable();
    }

    [Fact]
    public void ExponentialPeriodOverflowShouldBeMaximumExponentialPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(1000);
        _ = value.Should().Be(policy.MaximumExponentialPeriod);
    }

    [Theory]
    [InlineData(2, 2 * 60 * 1000)]
    [InlineData(5, 12 * 60 * 1000)]
    [InlineData(9, 88 * 60 * 1000)]
    [InlineData(15, 24 * 3600 * 1000)]
    [InlineData(70, 24 * 3600 * 1000)]
    [InlineData(90, 24 * 3600 * 1000)]
    [InlineData(200, 24 * 3600 * 1000)]
    public void ExponentialPeriodTimeRetryShouldBeExpectedValue(int sequence, int milliseconds)
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(sequence);
        _ = value.Should().Be(TimeSpan.FromMilliseconds(milliseconds));
    }

    [Fact]
    public void ExponentialPeriodTimeWithTwoRetriesShouldBeInitialPlusPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(2);
        _ = value.Should().Be(policy.InitialPeriod + policy.Period);
    }

    [Fact]
    public void ExponentialPeriodWithNoRetryShouldBeInitialPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(0);
        _ = value.Should().Be(policy.InitialPeriod);
    }

    [Fact]
    public void JsonSerializeSerializeShouldReturnSameValue()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = GetTestPolicy();
        string json = JsonSerializer.Serialize(policy);
        ResiliencyPolicy deserialized = JsonSerializer.Deserialize<ResiliencyPolicy>(json);
        _ = deserialized.Should().NotBeNull();
        _ = deserialized.Should().BeEquivalentTo(policy);
    }

    [Fact]
    public void LoadedPolicyShouldBeSameToSettings()
    {
        OptionsBuilder<ResiliencyTestSettings> builder = new OptionsBuilder<ResiliencyTestSettings>().WithValueFromConfiguration<ResiliencyPolicyTest>();
        ResiliencyTestSettings settings = builder.Build().Value;
        _ = settings.Dummy.Should().Be(100);
        _ = settings.TestResiliencyPolicy.Should().NotBeNull();

        _ = settings.TestResiliencyPolicy.Exponential.Should().BeTrue();
        _ = settings.TestResiliencyPolicy.Exponential.Should().BeTrue();
        _ = settings.TestResiliencyPolicy.InitialPeriod.Should().Be(TimeSpan.FromMilliseconds(500));
        _ = settings.TestResiliencyPolicy.MaximumExponentialPeriod.Should().Be(TimeSpan.FromDays(2));
        _ = settings.TestResiliencyPolicy.MaximumRetries.Should().Be(200);
        _ = settings.TestResiliencyPolicy.Period.Should().Be(TimeSpan.FromSeconds(30));
        _ = settings.TestResiliencyPolicy.Timeout.Should().Be(TimeSpan.FromDays(60));
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(1, 5)]
    [InlineData(2, 5 + 10)]
    [InlineData(3, 20 + 15)]
    [InlineData(4, 30 + 35)]
    [InlineData(5, 50 + 65)]
    [InlineData(6, 80 + 115)]
    [InlineData(7, 130 + 195)]
    [InlineData(8, 210 + 325)]
    [InlineData(9, 340 + 535)]
    [InlineData(10, 550 + 875)]
    [InlineData(11, 890 + 1425)]
    public void WaitTimeForEachExponentialRetryShouldBeExpectedValue(int sequence, int value)
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
    [InlineData(1, 200)]
    [InlineData(2, 100 + 200)]
    [InlineData(3, 200 + 200)]
    [InlineData(4, 300 + 200)]
    [InlineData(5, 400 + 200)]
    [InlineData(6, 500 + 200)]
    [InlineData(7, 600 + 200)]
    [InlineData(8, 700 + 200)]
    [InlineData(9, 800 + 200)]
    [InlineData(10, 900 + 200)]
    [InlineData(11, 1000 + 200)]
    public void WaitTimeForEachLinearRetryShouldBeExpectedValue(int sequence, int value)
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

    [Fact]
    public void XmlSerializeSerializeShouldReturnSameValue()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = GetTestPolicy();
        _ = policy.Should().BeXmlSerializable();
    }

    private static ResiliencyPolicy GetTestPolicy()
    {
        return new(
            100,
            TimeSpan.FromMilliseconds(200),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromHours(24),
            TimeSpan.FromDays(30),
            exponential: false);
    }
}

public class ResiliencyTestSettings : ISettings
{
    public int Dummy { get; set; }

    public ResiliencyPolicy TestResiliencyPolicy { get; set; }

    public static string ConfigurationName() => "ResiliencyTest";
}
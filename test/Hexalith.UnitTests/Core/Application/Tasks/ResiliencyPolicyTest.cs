// <copyright file="ResiliencyPolicyTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System.Text.Json;

using Shouldly;

using Hexalith.Application.Tasks;
using Hexalith.Extensions.Configuration;
using Hexalith.TestMocks;

public class ResiliencyPolicyTest
{
    [Fact]
    public void CanRetryShouldReturnCompletedIfExceedsTimeout()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        RetryStatus value = policy.CanRetry(DateTimeOffset.Now.Add(-policy.Timeout).AddSeconds(-1), 1);
        value.ShouldBe(RetryStatus.Stopped);
    }

    [Fact]
    public void CanRetryShouldReturnEnabledWhenExceedingRetryPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        RetryStatus value = policy.CanRetry(DateTimeOffset.Now.AddSeconds(-100), 1);
        value.ShouldBe(RetryStatus.Enabled);
    }

    [Fact]
    public void CanRetryShouldReturnSuspendedWhenNotExceedingRetryPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        RetryStatus value = policy.CanRetry(DateTimeOffset.Now.AddSeconds(-10), 1);
        value.ShouldBe(RetryStatus.Suspended);
    }

    [Fact]
    public void DataContractSerializeSerializeShouldReturnSameValue()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = GetTestPolicy();
        // Test DataContract serialization manually
        using System.IO.MemoryStream stream = new();
        System.Runtime.Serialization.DataContractSerializer serializer = new(typeof(ResiliencyPolicy));
        serializer.WriteObject(stream, policy);
        stream.Position = 0;
        ResiliencyPolicy deserialized = (ResiliencyPolicy)serializer.ReadObject(stream);
        deserialized.ShouldNotBeNull();
        deserialized.MaximumRetries.ShouldBe(policy.MaximumRetries);
    }

    [Fact]
    public void ExponentialPeriodOverflowShouldBeMaximumExponentialPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(1000);
        value.ShouldBe(policy.MaximumExponentialPeriod);
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
        value.ShouldBe(TimeSpan.FromMilliseconds(milliseconds));
    }

    [Fact]
    public void ExponentialPeriodTimeWithTwoRetriesShouldBeInitialPlusPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(2);
        value.ShouldBe(policy.InitialPeriod + policy.Period);
    }

    [Fact]
    public void ExponentialPeriodWithNoRetryShouldBeInitialPeriod()
    {
        ResiliencyPolicy policy = ResiliencyPolicy.CreateDefaultExponentialRetry();
        TimeSpan value = policy.EvaluatePeriod(0);
        value.ShouldBe(policy.InitialPeriod);
    }

    [Fact]
    public void JsonSerializeSerializeShouldReturnSameValue()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = GetTestPolicy();
        string json = JsonSerializer.Serialize(policy);
        ResiliencyPolicy deserialized = JsonSerializer.Deserialize<ResiliencyPolicy>(json);
        deserialized.ShouldNotBeNull();
        deserialized.MaximumRetries.ShouldBe(policy.MaximumRetries);
        deserialized.Period.ShouldBe(policy.Period);
    }

    [Fact]
    public void LoadedPolicyShouldBeSameToSettings()
    {
        OptionsBuilder<ResiliencyTestSettings> builder = new OptionsBuilder<ResiliencyTestSettings>().WithValueFromConfiguration<ResiliencyPolicyTest>();
        ResiliencyTestSettings settings = builder.Build().Value;
        settings.Dummy.ShouldBe(100);
        settings.TestResiliencyPolicy.ShouldNotBeNull();

        settings.TestResiliencyPolicy.Exponential.ShouldBeTrue();
        settings.TestResiliencyPolicy.Exponential.ShouldBeTrue();
        settings.TestResiliencyPolicy.InitialPeriod.ShouldBe(TimeSpan.FromMilliseconds(500));
        settings.TestResiliencyPolicy.MaximumExponentialPeriod.ShouldBe(TimeSpan.FromDays(2));
        settings.TestResiliencyPolicy.MaximumRetries.ShouldBe(200);
        settings.TestResiliencyPolicy.Period.ShouldBe(TimeSpan.FromSeconds(30));
        settings.TestResiliencyPolicy.Timeout.ShouldBe(TimeSpan.FromDays(60));
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
        waitMilliseconds.ShouldBe(value);
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
        waitMilliseconds.ShouldBe(value);
    }

    [Fact]
    public void XmlSerializeSerializeShouldReturnSameValue()
    {
        // Serialize resiliency policy
        ResiliencyPolicy policy = GetTestPolicy();
        // Test XML serialization manually
        using System.IO.MemoryStream stream = new();
        System.Runtime.Serialization.DataContractSerializer serializer = new(typeof(ResiliencyPolicy));
        serializer.WriteObject(stream, policy);
        stream.Position = 0;
        ResiliencyPolicy deserialized = (ResiliencyPolicy)serializer.ReadObject(stream);
        deserialized.ShouldNotBeNull();
        deserialized.MaximumRetries.ShouldBe(policy.MaximumRetries);
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
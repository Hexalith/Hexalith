// <copyright file="SettingsExceptionTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Exceptions;

using FluentAssertions;

using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Options;

public class SettingsExceptionTest
{
    [Fact]
    public void DefinedSettingsInOptionsShouldNotThrowException()
    {
        IOptions<DummySettings> settings = Options.Create(new DummySettings() { Name = "hello world" });
        SettingsException<DummySettings>.ThrowIfUndefined(settings.Value.Name);
    }

    [Fact]
    public void DefinedSettingsShouldNotThrowException()
    {
        DummySettings settings = new() { Name = "hello world" };
        SettingsException<DummySettings>.ThrowIfUndefined(settings.Name);
    }

    [Fact]
    public void NullSettingsPropertyInOptionsShouldThrowException()
    {
        IOptions<DummySettings> options = Options.Create<DummySettings>(new DummySettings());
        Action a = () => SettingsException<DummySettings>.ThrowIfUndefined(options.Value.Name);
        _ = a
            .Should()
            .Throw<SettingsException<DummySettings>>()
            .Where(p => p.ParamName == "options.Value.Name" && p.Message.Contains("Dummy") && p.Message.Contains("Name"));
    }

    [Fact]
    public void NullSettingsPropertyShouldThrowException()
    {
        DummySettings settings = new();
        Action a = () => SettingsException<DummySettings>.ThrowIfUndefined(settings.Name);
        _ = a
            .Should()
            .Throw<SettingsException<DummySettings>>()
            .Where(p => p.ParamName == "settings.Name" && p.Message.Contains("Dummy") && p.Message.Contains("Name"));
    }

    [Fact]
    public void NullSettingsSubPropertyShouldThrowException()
    {
        DummySettings settings = new() { SubConfig = new SubConfiguration() };
        Action a = () => SettingsException<DummySettings>.ThrowIfUndefined(settings.SubConfig.Hello);
        _ = a
            .Should()
            .Throw<SettingsException<DummySettings>>()
            .Where(p => p.ParamName == "settings.SubConfig.Hello" && p.Message.Contains("Dummy") && p.Message.Contains("Hello"));
    }

    internal class DummySettings : ISettings
    {
        public string Name { get; set; }

        public SubConfiguration SubConfig { get; set; }

        public static string ConfigurationName() => "Dummy";
    }

    internal class SubConfiguration
    {
        public string Hello { get; set; }
    }
}
// <copyright file="SettingsExceptionTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Exceptions;

using Shouldly;

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
        SettingsException<DummySettings> ex = Should.Throw<SettingsException<DummySettings>>(a);
        ex.ParamName.ShouldBe("options.Value.Name");
        ex.Message.ShouldContain("Dummy");
        ex.Message.ShouldContain("Name");
    }

    [Fact]
    public void NullSettingsPropertyShouldThrowException()
    {
        DummySettings settings = new();
        Action a = () => SettingsException<DummySettings>.ThrowIfUndefined(settings.Name);
        SettingsException<DummySettings> ex = Should.Throw<SettingsException<DummySettings>>(a);
        ex.ParamName.ShouldBe("settings.Name");
        ex.Message.ShouldContain("Dummy");
        ex.Message.ShouldContain("Name");
    }

    [Fact]
    public void NullSettingsSubPropertyShouldThrowException()
    {
        DummySettings settings = new() { SubConfig = new SubConfiguration() };
        Action a = () => SettingsException<DummySettings>.ThrowIfUndefined(settings.SubConfig.Hello);
        SettingsException<DummySettings> ex = Should.Throw<SettingsException<DummySettings>>(a);
        ex.ParamName.ShouldBe("settings.SubConfig.Hello");
        ex.Message.ShouldContain("Dummy");
        ex.Message.ShouldContain("Hello");
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
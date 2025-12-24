// <copyright file="CommandBusSettingsTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

using Shouldly;

public class CommandBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new CommandBusSettings().Name;
        name.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        CommandBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new CommandBusSettings());
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<CommandBusSettings> settings = new OptionsBuilder<CommandBusSettings>()
            .WithValueFromConfiguration<CommandBusSettingsTest>();
        string name = settings.Build().Value.Name;

        name.ShouldBe("my-command-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        CommandBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new CommandBusSettings() { Name = string.Empty });
        result.IsValid.ShouldBeFalse();
    }
}
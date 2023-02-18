// <copyright file="CommandBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using FluentAssertions;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

public class CommandBusSettingsTest
{
    [Fact]
    public void Check_default_name_is_not_null_or_empty()
    {
        string name = new CommandBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Check_default_values_validation()
    {
        CommandBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new CommandBusSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Check_load_from_configuration_has_correct_value()
    {
        OptionsBuilder<CommandBusSettings> settings = new OptionsBuilder<CommandBusSettings>()
            .WithValueFromConfiguration<CommandBusSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-command-bus");
    }

    [Fact]
    public void Validation_of_incorrect_values_should_fail()
    {
        CommandBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new CommandBusSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}
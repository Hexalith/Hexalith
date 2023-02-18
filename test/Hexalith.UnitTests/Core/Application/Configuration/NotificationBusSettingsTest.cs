// <copyright file="NotificationBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Configuration;

using FluentAssertions;

using Hexalith.Application.Buses;
using Hexalith.Application.Configuration;
using Hexalith.TestMocks;

public class NotificationBusSettingsTest
{
    [Fact]
    public void Check_default_name_is_not_null_or_empty()
    {
        string name = new NotificationBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Check_load_from_configuration_has_correct_value()
    {
        OptionsBuilder<NotificationBusSettings> settings = new OptionsBuilder<NotificationBusSettings>()
            .WithValueFromConfiguration<NotificationBusSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-notification-bus");
    }

    [Fact]
    public void Check_default_values_validation()
    {
        NotificationBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new NotificationBusSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validation_of_incorrect_values_should_fail()
    {
        NotificationBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new NotificationBusSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}

// <copyright file="NotificationBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using FluentAssertions;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

public class NotificationBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new NotificationBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        NotificationBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new NotificationBusSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<NotificationBusSettings> settings = new OptionsBuilder<NotificationBusSettings>()
            .WithValueFromConfiguration<NotificationBusSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-notification-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        NotificationBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new NotificationBusSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}
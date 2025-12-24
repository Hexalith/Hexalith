// <copyright file="NotificationBusSettingsTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

using Shouldly;

public class NotificationBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new NotificationBusSettings().Name;
        name.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        NotificationBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new NotificationBusSettings());
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<NotificationBusSettings> settings = new OptionsBuilder<NotificationBusSettings>()
            .WithValueFromConfiguration<NotificationBusSettingsTest>();
        string name = settings.Build().Value.Name;

        name.ShouldBe("my-notification-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        NotificationBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new NotificationBusSettings() { Name = string.Empty });
        result.IsValid.ShouldBeFalse();
    }
}
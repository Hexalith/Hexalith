// <copyright file="EventBusSettingsTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

using Shouldly;

public class EventBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new EventBusSettings().Name;
        name.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        EventBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new EventBusSettings());
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<EventBusSettings> settings = new OptionsBuilder<EventBusSettings>()
            .WithValueFromConfiguration<EventBusSettingsTest>();
        string name = settings.Build().Value.Name;

        name.ShouldBe("my-event-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        EventBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new EventBusSettings() { Name = string.Empty });
        result.IsValid.ShouldBeFalse();
    }
}
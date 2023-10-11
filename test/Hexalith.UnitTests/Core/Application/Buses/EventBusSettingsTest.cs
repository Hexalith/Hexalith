// <copyright file="EventBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using FluentAssertions;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

public class EventBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new EventBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        EventBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new EventBusSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<EventBusSettings> settings = new OptionsBuilder<EventBusSettings>()
            .WithValueFromConfiguration<EventBusSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-event-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        EventBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new EventBusSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}
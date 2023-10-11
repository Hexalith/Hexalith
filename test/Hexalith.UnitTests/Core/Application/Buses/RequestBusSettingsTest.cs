// <copyright file="RequestBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using FluentAssertions;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

public class RequestBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new RequestBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        RequestBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new RequestBusSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<RequestBusSettings> settings = new OptionsBuilder<RequestBusSettings>()
            .WithValueFromConfiguration<RequestBusSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-request-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        RequestBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new RequestBusSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}
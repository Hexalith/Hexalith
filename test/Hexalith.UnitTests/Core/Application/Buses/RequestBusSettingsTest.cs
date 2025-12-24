// <copyright file="RequestBusSettingsTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Buses;

using Hexalith.Application.Buses;
using Hexalith.TestMocks;

using Shouldly;

public class RequestBusSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new RequestBusSettings().Name;
        name.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        RequestBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new RequestBusSettings());
        result.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<RequestBusSettings> settings = new OptionsBuilder<RequestBusSettings>()
            .WithValueFromConfiguration<RequestBusSettingsTest>();
        string name = settings.Build().Value.Name;

        name.ShouldBe("my-request-bus");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        RequestBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new RequestBusSettings() { Name = string.Empty });
        result.IsValid.ShouldBeFalse();
    }
}
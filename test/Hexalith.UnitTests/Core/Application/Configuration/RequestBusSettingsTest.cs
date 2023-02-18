// <copyright file="RequestBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Configuration;

using FluentAssertions;

using Hexalith.Application.Buses;
using Hexalith.Application.Configuration;
using Hexalith.TestMocks;

public class RequestBusSettingsTest
{
    [Fact]
    public void Check_default_name_is_not_null_or_empty()
    {
        string name = new RequestBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Check_load_from_configuration_has_correct_value()
    {
        OptionsBuilder<RequestBusSettings> settings = new OptionsBuilder<RequestBusSettings>()
            .WithValueFromConfiguration<RequestBusSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-request-bus");
    }

    [Fact]
    public void Check_default_values_validation()
    {
        RequestBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new RequestBusSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validation_of_incorrect_values_should_fail()
    {
        RequestBusSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new RequestBusSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}

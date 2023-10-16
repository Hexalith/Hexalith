// <copyright file="StateStoreSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.States;

using FluentAssertions;

using Hexalith.Application.States;
using Hexalith.TestMocks;

public class StateStoreSettingsTest
{
    [Fact]
    public void CheckDefaultNameIsNotNullOrEmpty()
    {
        string name = new StateStoreSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void CheckDefaultValuesValidation()
    {
        StateStoreSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new StateStoreSettings());
        _ = result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void CheckLoadFromConfigurationHasCorrectValue()
    {
        OptionsBuilder<StateStoreSettings> settings = new OptionsBuilder<StateStoreSettings>()
            .WithValueFromConfiguration<StateStoreSettingsTest>();
        string name = settings.Build().Value.Name;

        _ = name.Should().Be("my-statestore");
    }

    [Fact]
    public void ValidationOfIncorrectValuesShouldFail()
    {
        StateStoreSettingsValidator validator = new();
        FluentValidation.Results.ValidationResult result = validator.Validate(new StateStoreSettings() { Name = string.Empty });
        _ = result.IsValid.Should().BeFalse();
    }
}
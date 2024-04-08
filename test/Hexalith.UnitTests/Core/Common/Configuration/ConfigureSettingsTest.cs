// <copyright file="ConfigureSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Configuration;

using FluentAssertions;

using Hexalith.TestMocks;

public class ConfigureSettingsTest
{
    [Fact]
    public void RetrieveSettingsShouldSucceed()
    {
        TestSettings expected = new()
        {
            TestLong = 101L,
            TestString = "Test string",
            TestClass = new TestClassValue
            {
                TestLong = 1001L,
                TestString = "Test class string",
            },
        };

        // See appsettings.json for values
        Microsoft.Extensions.Options.IOptions<TestSettings> settings = new OptionsBuilder<TestSettings>()
            .WithValueFromConfiguration<ConfigureSettingsTest>()
            .Build();
        _ = settings.Value.Should().BeEquivalentTo(expected);
    }
}
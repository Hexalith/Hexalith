// <copyright file="ConfigureSettingsTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Configuration;

using Shouldly;

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
        settings.Value.TestLong.ShouldBe(expected.TestLong);
        settings.Value.TestString.ShouldBe(expected.TestString);
        settings.Value.TestClass.TestLong.ShouldBe(expected.TestClass.TestLong);
        settings.Value.TestClass.TestString.ShouldBe(expected.TestClass.TestString);
    }
}
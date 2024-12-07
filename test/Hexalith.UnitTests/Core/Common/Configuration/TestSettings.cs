// <copyright file="TestSettings.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Configuration;

using Hexalith.Extensions.Configuration;

public class TestSettings : ISettings
{
    public TestClassValue TestClass { get; set; }

    public long TestLong { get; set; }

    public string TestString { get; set; }

    public static string ConfigurationName() => "Test";
}
// <copyright file="CommandBusSettingsTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Configuration;

using FluentAssertions;

using Hexalith.Application.Configuration;

public class CommandBusSettingsTest
{
    [Fact]
    public void Check_default_name_is_not_null_or_empty()
    {
        string name = new CommandBusSettings().Name;
        _ = name.Should().NotBeNullOrEmpty();
    }
}

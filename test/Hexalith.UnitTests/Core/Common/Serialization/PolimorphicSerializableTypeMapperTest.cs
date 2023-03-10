// <copyright file="PolimorphicSerializableTypeMapperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System;
using System.Collections.Generic;

using FluentAssertions;

using Hexalith.Extensions.Serialization;

public class PolimorphicSerializableTypeMapperTest
{
    [Fact]
    public void Map_should_succeed()
    {
        Dictionary<string, Type> map = PolymorphicSerializableTypeMapper.GetMap();
        _ = map.Should().NotBeNull();
    }
}
// <copyright file="TestHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests;

using System.Linq;

using Hexalith.PolymorphicSerialization;
using Hexalith.UnitTests.Core.Application.Commands;
using Hexalith.UnitTests.Extensions;

public static class TestHelper
{
    public static void AddSerializationMappers()
    {
        if (!PolymorphicSerializationResolver
                  .DefaultMappers
                  .Any(c => c.JsonDerivedType.DerivedType == typeof(DummyCommand1)))
        {
            PolymorphicSerializationResolver.DefaultMappers = PolymorphicSerializationResolver
                .DefaultMappers
                .AddHexalithUnitTestsMappers();
        }
    }
}
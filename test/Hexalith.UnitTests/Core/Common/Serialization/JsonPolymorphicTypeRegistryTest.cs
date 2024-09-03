// <copyright file="JsonPolymorphicTypeRegistryTest.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System.Text.Json.Serialization.Metadata;

using FluentAssertions;

using Hexalith.Extensions.Serialization;

public static class JsonPolymorphicTypeRegistryTest
{
    [Fact]
    public static void Get_polymorphic_base_type_on_base_type_should_return_type()
    {
        // Act
        Type result = JsonPolymorphicTypeRegistry.GetPolymorphicBaseType<TestMessageBase>();

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Should().Be(typeof(TestMessageBase));
    }

    [Fact]
    public static void Get_polymorphic_base_type_on_derived_type_should_return_abstract_base_type()
    {
        // Act
        Type result = JsonPolymorphicTypeRegistry.GetPolymorphicBaseType<TestMessageWithAbstractBase1>();

        // Assert
        _ = result.Should().Be(typeof(TestMessageAbstractBase));
    }

    [Fact]
    public static void Get_polymorphic_base_type_on_derived_type_should_return_base_type()
    {
        // Act
        Type result = JsonPolymorphicTypeRegistry.GetPolymorphicBaseType<TestMessage1>();

        // Assert
        _ = result.Should().Be(typeof(TestMessageBase));
    }

    [Fact]
    public static void Get_polymorphic_base_type_on_multi_level_derived_type_should_return_base_type()
    {
        // Act
        Type result = JsonPolymorphicTypeRegistry.GetPolymorphicBaseType<TestMessage3>();

        // Assert
        _ = result.Should().Be(typeof(TestMessageBase));
    }

    [Fact]
    public static void Register_derived_type_should_add_type_and_not_polymorphic_base_type()
    {
        // Prepare
        JsonPolymorphicTypeRegistry registry = new();

        // Act
        registry.RegisterJsonDerivedType<TestMessage1>();
        IEnumerable<JsonDerivedType> result = registry.GetDerivedTypes(typeof(TestMessageBase));

        // Assert
        _ = result.Should().HaveCount(1);
        _ = result.First().DerivedType.Should().Be(typeof(TestMessage1));
    }

    [Fact]
    public static void Register_multi_inheritence_derived_type_should_add_all_base_types()
    {
        // Prepare
        JsonPolymorphicTypeRegistry registry = new();

        // Act
        registry.RegisterJsonDerivedType<TestMessage3>();
        JsonDerivedType[] result = registry.GetDerivedTypes(typeof(TestMessageBase)).ToArray();

        // Assert
        _ = result.Should().HaveCount(2);
        _ = result[0].DerivedType.Should().Be(typeof(TestMessage2));
        _ = result[1].DerivedType.Should().Be(typeof(TestMessage3));
    }
}
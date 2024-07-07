// <copyright file="JsonPolymorphicSerializationTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System.Text.Json;

using FluentAssertions;

using Hexalith.Extensions.Serialization;

public static class JsonPolymorphicSerializationTest
{
    private static JsonSerializerOptions Options => new()
    {
        WriteIndented = true,
        TypeInfoResolver = new JsonPolymorphicTypeResolver(new JsonPolymorphicTypeRegistry()),
    };

    [Fact]
    public static void Deserialize_polymorphic_no_types_registered_using_base_type_should_throw_an_exception()
    {
        // Arrange
        TestMessage1 message = new("id123", "name123", "value1-123");

        // Act
        Action serialize = () => _ = JsonSerializer.Serialize(message);

        // Assert
        _ = serialize
            .Should()
            .Throw<InvalidOperationException>()
            .Where(p => p.Message.Contains(nameof(TestMessageBase)));
    }

    [Fact]
    public static void Deserialize_polymorphic_type_not_registered_using_base_type_should_throw_an_exception()
    {
        // Arrange
        TestMessage1 message = new("id123", "name123", "value1-123");
        JsonPolymorphicTypeRegistry registry = new();
        registry.RegisterJsonDerivedType<TestMessage2>();
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            TypeInfoResolver = new JsonPolymorphicTypeResolver(registry),
        };

        // Act
        Action serialize = () => _ = JsonSerializer.Serialize(message, options);

        // Assert
        _ = serialize
            .Should()
            .Throw<InvalidOperationException>()
            .Where(p => p.Message.Contains(nameof(TestMessageBase)));
    }

    [Fact]
    public static void Serialize_and_deserialize_registered_polymorphic_custom_name_type_not_registered_should_succeed()
    {
        // Arrange
        TestMessageWithCustomName message = new("id123", "name123", "value2-123");
        JsonPolymorphicTypeRegistry registry = new();
        registry.RegisterJsonDerivedType<TestMessageWithCustomName>();
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            TypeInfoResolver = new JsonPolymorphicTypeResolver(registry),
        };

        // Act
        string json = JsonSerializer.Serialize(message, options);
        TestMessageWithCustomName result = JsonSerializer.Deserialize<TestMessageWithCustomName>(json, options);

        // Assert
        _ = result.Should().BeEquivalentTo(message);
        _ = json.Should().Contain("$type");
        _ = json.Should().Contain("hello|V99");
    }

    [Fact]
    public static void Serialize_and_deserialize_registered_polymorphic_type_not_registered_should_succeed()
    {
        // Arrange
        TestMessage1 message = new("id123", "name123", "value1-123");

        // Act
        string json = JsonSerializer.Serialize(message, Options);
        TestMessage1 result = JsonSerializer.Deserialize<TestMessage1>(json, Options);

        // Assert
        _ = result.Should().BeEquivalentTo(message);
    }

    [Fact]
    public static void Serialize_polymorphic_type_not_registered_using_base_type_should_throw_an_exception()
    {
        // Arrange
        TestMessage1 message = new("id123", "name123", "value1-123");
        string json = """
            {
              "$type": "TestMessage1|V1",
              "Value1": "value1-123",
              "Id": "id123",
              "Name": "name123"
            }
            """;

        // Act
        _ = JsonSerializer.Serialize(message, Options);

        // Assert
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().NotContain("$type");
    }

    [Fact]
    public static void SerializePolymorphicObjectShouldSucceed()
    {
        // Arrange
        TestMessage1 message = new("id123", "name123", "value1-123");
        JsonPolymorphicTypeRegistry registry = new();
        registry.RegisterJsonDerivedType<TestMessage1>();
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            TypeInfoResolver = new JsonPolymorphicTypeResolver(registry),
        };

        // Act
        string json = JsonSerializer.Serialize(message, options);

        // Assert
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().Contain("TestMessage1");
        _ = json.Should().Contain("id123");
        _ = json.Should().Contain("name123");
        _ = json.Should().Contain("value1-123");
        _ = json.Should().Contain("$type");
    }

    [Fact]
    public static void SerializePolymorphicTypeNotRegisteredShouldSerializeWithoutTypeDefinition()
    {
        // Arrange
        TestMessage1 message = new("id123", "name123", "value1-123");

        // Act
        string json = JsonSerializer.Serialize(message, Options);

        // Assert
        _ = json.Should().NotBeNullOrEmpty();
        _ = json.Should().NotContain("$type");
    }
}
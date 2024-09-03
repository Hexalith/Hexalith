// <copyright file="JsonPolymorphicTypeResolverTest.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System.Text.Json.Serialization.Metadata;

using FluentAssertions;

using Hexalith.Extensions.Serialization;

using Moq;

public class JsonPolymorphicTypeResolverTest
{
    [Fact]
    public void Resolve_polymorphic_type_should_return_polymorphic_options()
    {
        // Arrange
        Mock<IJsonPolymorphicTypeRegistry> registry = new(MockBehavior.Strict);
        _ = registry.Setup(r
            => r.AddDerivedTypesToList(It.IsAny<Type>(), It.IsAny<IList<JsonDerivedType>>()));

        _ = registry.Setup(r
            => r.GetDerivedTypes(It.Is<Type>((t) => t == typeof(TestMessageBase))))
            .Returns(() => [new JsonDerivedType(typeof(TestMessage1))]);

        JsonPolymorphicTypeResolver resolver = new(registry.Object);

        // Act
        JsonTypeInfo result = resolver.GetTypeInfo(typeof(TestMessage1), new());

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Type.Should().Be(typeof(TestMessage1));
        _ = result.Options.Should().NotBeNull();
        _ = result.PolymorphismOptions.Should().NotBeNull();
    }

    [Fact]
    public void Resolve_TimeSpan_should_succeed()
    {
        // Arrange
        Mock<IJsonPolymorphicTypeRegistry> registry = new(MockBehavior.Strict);
        JsonPolymorphicTypeResolver resolver = new(registry.Object);

        // Act
        JsonTypeInfo result = resolver.GetTypeInfo(typeof(TimeSpan), new());

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Type.Should().Be(typeof(TimeSpan));
        _ = result.Options.Should().NotBeNull();
        _ = result.PolymorphismOptions.Should().BeNull();
    }
}
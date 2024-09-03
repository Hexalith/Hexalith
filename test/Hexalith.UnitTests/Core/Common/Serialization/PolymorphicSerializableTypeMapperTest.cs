// <copyright file="PolymorphicSerializableTypeMapperTest.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System.Collections.Frozen;

using FluentAssertions;

using Hexalith.Extensions.Reflections;
using Hexalith.Extensions.Serialization;

public class PolymorphicSerializableTypeMapperTest
{
    [Fact]
    public void CanCreateMappableTypeOnAbstractShouldBeFalse()
    {
        bool map = TypeMapper.CanCreateMappableType<IPolymorphicSerializable>(typeof(TestAbstractMessage));
        _ = map.Should().BeFalse();
    }

    [Fact]
    public void CanCreateMappableTypeShouldBeTrue()
    {
        bool map = TypeMapper.CanCreateMappableType<IPolymorphicSerializable>(typeof(TestMessage));
        _ = map.Should().BeTrue();
    }

    [Fact]
    public void IsMappableConcreteClassOnAbstractShouldBeFalse()
    {
        bool map = TypeMapper.IsMappableConcreteClass<IPolymorphicSerializable>(typeof(TestAbstractMessage));
        _ = map.Should().BeFalse();
    }

    [Fact]
    public void IsMappableConcreteClassShouldBeTrue()
    {
        bool map = TypeMapper.IsMappableConcreteClass<IPolymorphicSerializable>(typeof(TestMessage));
        _ = map.Should().BeTrue();
    }

    [Fact]
    public void MapShouldContainTestMessage()
    {
        FrozenDictionary<string, IPolymorphicSerializable> map = TypeMapper.GetMap<IPolymorphicSerializable>();
        _ = map.Should().NotBeNull();
        IPolymorphicSerializable message = map
            .FirstOrDefault(p => p.Value.GetType() == typeof(TestMessage)).Value;
        _ = message.Should().NotBeNull();
        _ = message.Should().BeOfType<TestMessage>();
        _ = map[message.TypeMapName].Should().BeOfType<TestMessage>();
    }

    [Fact]
    public void MapShouldNotContainNonPublicConstructorMessage()
    {
        FrozenDictionary<string, IPolymorphicSerializable> map = TypeMapper.GetMap<IPolymorphicSerializable>();
        _ = map.Should().NotBeNull();
        _ = map
            .FirstOrDefault(p => p.Value.GetType() == typeof(TestNonPublicConstructorMessage))
            .Value
            .Should()
            .BeNull();
    }

    [Fact]
    public void MapShouldNotContainTestAbstractMessage()
    {
        FrozenDictionary<string, IPolymorphicSerializable> map = TypeMapper.GetMap<IPolymorphicSerializable>();
        _ = map.Should().NotBeNull();
        _ = map
            .FirstOrDefault(p => p.Value.GetType() == typeof(TestAbstractMessage))
            .Value
            .Should()
            .BeNull();
    }

    public abstract class TestAbstractMessage : IPolymorphicSerializable
    {
        public int MajorVersion { get; } = 1;

        public int MinorVersion { get; } = 2;

        public string Name { get; set; } = "Test";

        public string TypeMapName => IPolymorphicSerializable.GetTypeMapName(TypeName, MajorVersion, MinorVersion);

        public string TypeName { get; } = nameof(TestMessage);
    }

    public class TestMessage : IPolymorphicSerializable
    {
        public int MajorVersion { get; } = 1;

        public int MinorVersion { get; } = 2;

        public string Name { get; set; } = "Test";

        public string TypeMapName => IPolymorphicSerializable.GetTypeMapName(TypeName, MajorVersion, MinorVersion);

        public string TypeName { get; } = nameof(TestMessage);
    }

    public class TestNonPublicConstructorMessage : IPolymorphicSerializable
    {
        protected TestNonPublicConstructorMessage()
        {
        }

        public int MajorVersion { get; } = 1;

        public int MinorVersion { get; } = 2;

        public string Name { get; set; } = "Test";

        public string TypeMapName => IPolymorphicSerializable.GetTypeMapName(TypeName, MajorVersion, MinorVersion);

        public string TypeName { get; } = nameof(TestMessage);
    }
}
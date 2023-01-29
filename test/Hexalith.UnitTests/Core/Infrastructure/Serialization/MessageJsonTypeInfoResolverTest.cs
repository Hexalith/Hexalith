// <copyright file="MessageJsonTypeInfoResolverTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Serialization;

using System.Runtime.Serialization;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Serialization;

public interface ITest
{
    string MyProp1 { get; set; }
}

[DataContract]
[JsonPolymorphicBaseClass]
public class Test1 : ITest
{
    public string MyProp1 { get; set; }
}

[DataContract]
public class Test2 : Test1
{
    public string MyProp2 { get; set; }
}

[DataContract]
public class Test3 : Test1
{
    public string MyProp3 { get; set; }
}

[DataContract]
public class Test4 : Test3
{
    public string MyProp4 { get; set; }
}

[DataContract]
[JsonPolymorphicBaseClass(DiscriminatorName = "MyDisc")]
public class TestCustom1 : ITest
{
    public string MyProp1 { get; set; }
}

[DataContract]
public class TestCustom2 : TestCustom1
{
    public string MyProp2 { get; set; }
}

public class MessageJsonTypeInfoResolverTest
{
    [Fact]
    public void Deserialize_object_with_custom_discriminator_json_should_succed()
    {
        string json = $$"""{ "MyDisc":"TestCustom2", "MyProp1":"P1", "MyProp2":"P2" }""";
        JsonSerializerOptions options = new()
        {
            TypeInfoResolver = new PolymorphicTypeResolver(),
        };
        TestCustom1 result = JsonSerializer.Deserialize<TestCustom1>(json, options);
        _ = result.Should().BeOfType<TestCustom2>();
        TestCustom2 t2 = (TestCustom2)result;
        _ = t2.MyProp1.Should().Be("P1");
        _ = t2.MyProp2.Should().Be("P2");
    }

    [Fact]
    public void Serialize_and_deserilise_should_return_same_object()
    {
        string json = JsonSerializer.Serialize<Test1>(
             new Test2 { MyProp1 = "P1", MyProp2 = "P2" },
             new JsonSerializerOptions { TypeInfoResolver = new PolymorphicTypeResolver() });
        Test1 result = JsonSerializer.Deserialize<Test1>(json, new JsonSerializerOptions { TypeInfoResolver = new PolymorphicTypeResolver() });
        _ = result.Should().BeOfType<Test2>();
    }

    [Fact]
    public void Serialize_and_deserilise_with_custom_discriminator_should_return_same_object()
    {
        TestCustom2 test2 = new() { MyProp1 = "P1", MyProp2 = "P2" };
        string json = JsonSerializer.Serialize<TestCustom1>(
             test2,
             new JsonSerializerOptions { TypeInfoResolver = new PolymorphicTypeResolver() });
        TestCustom1 result = JsonSerializer.Deserialize<TestCustom1>(json, new JsonSerializerOptions { TypeInfoResolver = new PolymorphicTypeResolver() });
        _ = result.Should().BeOfType<TestCustom2>();
        TestCustom2 t2 = (TestCustom2)result;
        _ = t2.MyProp1.Should().Be(test2.MyProp1);
        _ = t2.MyProp2.Should().Be(test2.MyProp2);
    }

    [Fact]
    public void Serialized_object_json_should_contain_object_type()
    {
        JsonSerializerOptions options = new()
        {
            TypeInfoResolver = new PolymorphicTypeResolver(),
        };
        string json = JsonSerializer.Serialize<Test1>(
            new Test2 { MyProp1 = "P1", MyProp2 = "P2" },
            options);
        _ = json.Should().Contain("\"$type\":\"Test2\"");
    }

    [Fact]
    public void Serialized_object_with_custom_discriminator_json_should_contain_discriminator()
    {
        JsonSerializerOptions options = new()
        {
            TypeInfoResolver = new PolymorphicTypeResolver(),
        };
        string json = JsonSerializer.Serialize<TestCustom1>(
            new TestCustom2 { MyProp1 = "P1", MyProp2 = "P2" },
            options);
        _ = json.Should().Contain("\"MyDisc\":\"TestCustom2\"");
    }
}
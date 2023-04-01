// <copyright file="PolymorphicJsonConverterTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

using FluentAssertions;
using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Serialization;

#pragma warning disable SA1649 // File name should match first type name

[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<TestBase>))]
public abstract class TestBase : IPolymorphicSerializable
{
    [IgnoreDataMember]
    [JsonIgnore]
    public int MajorVersion => DefaultMajorVersion();

    [IgnoreDataMember]
    [JsonIgnore]
    public int MinorVersion => DefaultMinorVersion();

    [IgnoreDataMember]
    [JsonIgnore]
    public string TypeName => DefaultTypeName();

    protected virtual int DefaultMajorVersion()
    {
        return 0;
    }

    protected virtual int DefaultMinorVersion()
    {
        return 0;
    }

    protected virtual string DefaultTypeName()
    {
        return GetType().Name;
    }
}

[DataContract]
public class TestCustom1 : TestBase
{
    public string MyProp1 { get; set; }
}

[DataContract]
public class TestCustom2 : TestCustom1
{
    public string MyProp2 { get; set; }
}

public class PolymorphicJsonConverterTest
{
    [Fact]
    public void Deserialize_polymorphic_object_should_succeed()
    {
        string json = $$"""{ "MyDisc":"TestCustom2", "MyProp1":"P1", "MyProp2":"P2", "$type_name":"TestCustom2" }""";
        TestBase result = JsonSerializer.Deserialize<TestBase>(json);
        _ = result.Should().BeOfType<TestCustom2>();
        TestCustom2 t2 = (TestCustom2)result;
        _ = t2.MyProp1.Should().Be("P1");
        _ = t2.MyProp2.Should().Be("P2");
    }

    [Fact]
    public void Polymorphic_serialize_and_deserialize_should_return_same_object()
    {
        TestCustom2 test2 = new() { MyProp1 = "P1", MyProp2 = "P2" };
        string json = JsonSerializer.Serialize<TestBase>(test2);
        TestBase result = JsonSerializer.Deserialize<TestBase>(json);
        _ = result.Should().BeOfType<TestCustom2>();
        _ = result.Should().BeEquivalentTo(test2);
    }
}
// <copyright file="MessageJsonTypeInfoResolverTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.Serialization;

using FluentAssertions;

using Hexalith.Extensions.Serialization;
using Hexalith.Infrastructure.Serialization;

using System.Runtime.Serialization;
using System.Text.Json;

#pragma warning disable MA0048 // File name must match type name

public interface ITest
{
	string? MyProp1 { get; set; }
}

[DataContract]
[JsonPolymorphicBaseClass]
public class Test : ITest
{
	public string? MyProp1 { get; set; }
}

[DataContract]
public class Test2 : Test
{
	public string? MyProp2 { get; set; }
}

[DataContract]
public class Test3 : Test
{
	public string? MyProp3 { get; set; }
}

[DataContract]
public class Test4 : Test3
{
	public string? MyProp4 { get; set; }
}

public class MessageJsonTypeInfoResolverTest
{
	[Fact]
	public void Serialize_and_deserilise_should_return_same_object()
	{
		string json = JsonSerializer.Serialize<Test>(
			 new Test2 { MyProp1 = "P1", MyProp2 = "P2" },
			 new JsonSerializerOptions() { TypeInfoResolver = new PolymorphicTypeResolver() });
		Test? result = JsonSerializer.Deserialize<Test>(json, new JsonSerializerOptions() { TypeInfoResolver = new PolymorphicTypeResolver() });
		_ = result.Should().BeOfType<Test2>();
	}

	[Fact]
	public void Serialized_object_json_should_contain_object_type()
	{
		JsonSerializerOptions options = new()
		{
			TypeInfoResolver = new PolymorphicTypeResolver(),
		};
		string json = JsonSerializer.Serialize<Test>(
			new Test2 { MyProp1 = "P1", MyProp2 = "P2" },
			options);
		_ = json.Should().Contain("\"$type\":\"Test2\"");
	}
}
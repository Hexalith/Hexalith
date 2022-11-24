// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Core.Common.Extensions;

using FluentAssertions;

using Hexalith.Extensions.Common;

public class ArrayHelperTest
{
	[Fact]
	public void Interger_into_array_should_contain_value() => Into_array_should_contain_value(101);

	[Fact]
	public void List_into_array_should_contain_integer_value() => Into_array_should_contain_value(new List<string>() { "Hello once", "Hello again " });

	[Fact]
	public void Object_into_array_should_contain_value() => Into_array_should_contain_value(new { Hello = "Hello", Count = 100 });

	[Fact]
	public void String_into_array_should_contain_value() => Into_array_should_contain_value("Hello world!");

	private static void Into_array_should_contain_value<T>(T value)
	{
		T[] array = value.IntoArray();
		_ = array.Should().ContainSingle();
		_ = array[0].Should().Be(value);
	}
}
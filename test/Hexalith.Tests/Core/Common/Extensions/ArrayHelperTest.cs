// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Core.Common.Extensions;

using FluentAssertions;

using Hexalith.Extensions.Common;

public class ArrayHelperTest
{
	[Fact]
	public void Interger_into_array_should_contain_value()
	{
		List<string>[] array = new List<string> { "Hello once", "Hello again " }.IntoArray();
		_ = array.Should().ContainSingle();
		_ = array[0][0].Should().Be("Hello once");
		_ = array[0][1].Should().Be("Hello again ");
	}

	[Fact]
	public void List_into_array_should_contain_integer_value()
	{
		int[] array = 1254.IntoArray();
		_ = array.Should().ContainSingle();
		_ = array[0].Should().Be(1254);
	}

	[Fact]
	public void Object_into_array_should_contain_value()
	{
		var array = new { Hello = "Hello", Count = 100 }.IntoArray();
		_ = array.Should().ContainSingle();
		_ = array[0].Hello.Should().Be("Hello");
		_ = array[0].Count.Should().Be(100);
	}

	[Fact]
	public void String_into_array_should_contain_value()
	{
		string[] array = "Hello".IntoArray();
		_ = array.Should().ContainSingle();
		_ = array[0].Should().Be("Hello");
	}
}
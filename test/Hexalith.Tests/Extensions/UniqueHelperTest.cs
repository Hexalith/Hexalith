// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Extensions;

using FluentAssertions;

using Hexalith.Extensions;

public class UniqueHelperTest
{
	[Fact]
	public void Get_a_thousand_unique_id_without_any_duplicates()
	{
		HashSet<string> ids = new();
		for (int i = 0; i < 1000; i++)
		{
			_ = ids.Add(UniqueIdHelper.GetUniqueStringId());
		}
	}

	[Fact]
	public void Get_unique_id_returns_22_chars()
	{
		string id = UniqueIdHelper.GetUniqueStringId();
		_ = id.Should().HaveLength(22, "Because the id is a base64 string of 16 bytes");
	}
}
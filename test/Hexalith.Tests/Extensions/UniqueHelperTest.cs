// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Tests.Extensions;

using Hexalith.Extensions;

public class UniqueHelperTest
{
	[Fact]
	public void Get_unique_id_returns_22_chars()
	{
		string id = UniqueIdHelper.GetUniqueStringId();
		Assert.Equal(22, id.Length);
	}
}
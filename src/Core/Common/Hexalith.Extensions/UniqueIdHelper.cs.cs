// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Extensions;

public class UniqueIdHelper
{
	public static string GetUniqueStringId()
	{
		return Convert
				.ToBase64String(Guid.NewGuid().ToByteArray())[..22]
				.Replace("/", "_", StringComparison.InvariantCulture)
				.Replace("+", "-", StringComparison.InvariantCulture);
	}
}
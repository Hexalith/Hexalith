// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Extensions;

/// <summary>
/// Unique Id generators
/// </summary>
public class UniqueIdHelper
{
	private static readonly object _lock = new();
	private static string? _previousDateTimeId;

	/// <summary>
	/// Generate a new unique id of 17 chararacters from the current date time (yyyyMMddHHmmssfff). Only one Id per millisecond can be generated.
	/// </summary>
	/// <returns>Id string</returns>
	public static string GenerateDateTimeId()
	{
		lock (_lock)
		{
			string value = DateTime.Now.ToString("yyyyMMddHHmmssfff");
			if (value == _previousDateTimeId)
			{
				Task.Delay(1).Wait();
				return GenerateDateTimeId();
			}
			_previousDateTimeId = value;
			return value;
		}
	}

	/// <summary>
	/// Generate a new unique id of 22 chararacters
	/// </summary>
	/// <returns>Id string</returns>
	public static string GenerateUniqueStringId()
	{
		return Convert
				.ToBase64String(Guid.NewGuid().ToByteArray())[..22]
				.Replace("/", "_", StringComparison.InvariantCulture)
				.Replace("+", "-", StringComparison.InvariantCulture);
	}
}
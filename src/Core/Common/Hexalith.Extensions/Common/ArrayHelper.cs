// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Extensions.Common;

public static class ArrayHelper
{
	/// <summary>
	/// Creates an array containing the object.
	/// </summary>
	/// <typeparam name="T">Type of the object</typeparam>
	/// <param name="obj">Instance of the object</param>
	/// <returns>An array containing the object</returns>
	public static T[] IntoArray<T>(this T obj) => new[] { obj };
}
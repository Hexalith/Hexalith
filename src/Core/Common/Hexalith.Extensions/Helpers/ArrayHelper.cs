// <copyright file="ArrayHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Helpers;

/// <summary>
/// Array helpers.
/// </summary>
public static class ArrayHelper
{
	/// <summary>
	/// Creates an array containing the object.
	/// </summary>
	/// <typeparam name="T">Type of the object.</typeparam>
	/// <param name="obj">Instance of the object.</param>
	/// <returns>An array containing the object.</returns>
	public static T[] IntoArray<T>(this T obj)
	{
		return new[] { obj };
	}
}
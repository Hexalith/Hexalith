// <copyright file="LoggerBuilder.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using Microsoft.Extensions.Logging;

using Moq;

/// <summary>
/// Helper class to build a <see cref="ILogger"/> mock.
/// </summary>
/// <typeparam name="T">The type of logger (ILogger<typeparamref name="T"/>).</typeparam>
public class LoggerBuilder<T>
{
	/// <summary>
	/// Build a <see cref="ILogger{T}"/>.
	/// </summary>
	/// <returns>The mocked logger.</returns>
	public ILogger<T> Build()
	{
		return BuildMock().Object;
	}

	/// <summary>
	/// Build a <see cref="Mock{ILogger{T}}"/>.
	/// </summary>
	/// <returns>The mock of ILogger.</returns>
	public Mock<ILogger<T>> BuildMock()
	{
		return new();
	}
}
// <copyright file="LoggerBuilder.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using Microsoft.Extensions.Logging;

using Moq;

/// <summary>
/// Helper class to build a <see cref="ILogger"/> mock.
/// </summary>
/// <typeparam name="T">The type of logger (ILogger&lt;typeparamref name="T"/>&gt;).</typeparam>
public class LoggerBuilder<T> : IMockBuilder<ILogger<T>>
{
    /// <summary>
    /// Build a <see cref="ILogger{T}"/>.
    /// </summary>
    /// <returns>The mocked logger.</returns>
    public ILogger<T> Build() => BuildMock().Object;

    /// <summary>
    /// Build.
    /// </summary>
    /// <returns>The mock of ILogger.</returns>
    public IMock<ILogger<T>> BuildMock() => new Mock<ILogger<T>>();
}
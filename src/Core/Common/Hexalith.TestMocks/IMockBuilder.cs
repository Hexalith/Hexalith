// <copyright file="IMockBuilder.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using Moq;

/// <summary>
/// Interface for mock builders.
/// </summary>
/// <typeparam name="T">The type of the mocked interface.</typeparam>
public interface IMockBuilder<out T>
    where T : class
{
    /// <summary>
    /// Build a mocked instance of <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The mocked instance.</returns>
    T Build();

    /// <summary>
    /// Build a <see cref="Mock{T}"/> of <typeparamref name="T"/>.
    /// </summary>
    /// <returns>The mock of the interface.</returns>
    IMock<T> BuildMock();
}
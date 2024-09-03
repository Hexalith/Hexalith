// ***********************************************************************
// Assembly         : Hexalith.TestMocks
// Author           : Jérôme Piquot
// Created          : 12-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-04-2023
// ***********************************************************************
// <copyright file="SerializationTestBase.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.TestMocks;

using FluentAssertions;

using Xunit;

/// <summary>
/// Class SerializationTestBase.
/// </summary>
public abstract class SerializationTestBase
{
    /// <summary>
    /// Defines the test method CheckDataContractSerialization.
    /// </summary>
    [Fact]
    public void CheckDataContractSerialization() => _ = ToSerializeObject().Should().BeDataContractSerializable();

    /// <summary>
    /// Defines the test method CheckJsonSerialization.
    /// </summary>
    [Fact]
    public void CheckJsonSerialization() => _ = ToSerializeObject().Should().BeJsonSerializable(string.Empty);

    /// <summary>
    /// Converts to serialize object.
    /// </summary>
    /// <returns>System.Object.</returns>
    public abstract object ToSerializeObject();
}
// <copyright file="PolymorphicSerializationTestBase.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.TestMocks;

using System.Text.Json;

using Shouldly;

using Xunit;

/// <summary>
/// Class PolymorphicSerializationTestBase.
/// Implements the <see cref="Hexalith.TestMocks.SerializationTestBase" />.
/// </summary>
/// <typeparam name="TObject">The type of the t object.</typeparam>
/// <typeparam name="TBase">The type of the t base.</typeparam>
/// <seealso cref="Hexalith.TestMocks.SerializationTestBase" />
public abstract class PolymorphicSerializationTestBase<TObject, TBase> : SerializationTestBase
    where TObject : class, TBase
    where TBase : class
{
    /// <summary>
    /// Defines the test method PolymorphicSerializeAndDeserializeShouldReturnSameObject.
    /// </summary>
    [Fact]
    public void PolymorphicSerializeAndDeserializeShouldReturnSameObject()
    {
        TObject original = (TObject)ToSerializeObject();
        string json = JsonSerializer.Serialize<TBase>(original);
        TBase? result = JsonSerializer.Deserialize<TBase>(json);
        result.ShouldNotBeNull();
        result.ShouldBeOfType<TObject>();
        result.ShouldBeEquivalentTo(original);
    }
}
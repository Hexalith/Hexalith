// <copyright file="AreSameTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class AreSameTest
{
    [Fact]
    public void EmbeddedEquatableAreSameShouldReturnTrue()
    {
        DummyEmbeddedEquatable a = new();
        DummyEmbeddedEquatable b = new() { Property4 = a.Property4 };
        _ = a.AreSame(b).Should().BeTrue();
    }

    [Fact]
    public void EquatableAreSameShouldReturnFalse()
    {
        DummyEquatable a = new();
        DummyEquatable b = new() { Property2 = "Hello" };
        _ = a.AreSame(b).Should().BeFalse();
    }

    [Fact]
    public void EquatableAreSameShouldReturnTrue()
    {
        DummyEquatable a = new();
        DummyEquatable b = new();
        _ = a.AreSame(b).Should().BeTrue();
    }

    [Fact]
    public void NonEquatableAreSameShouldReturnFalse()
    {
        DummyNonEquatable a = new();
        DummyNonEquatable b = new();
        _ = a.AreSame(b).Should().BeFalse();
    }

    [Fact]
    public void NonEquatableAreSameShouldReturnTrue()
    {
        DummyNonEquatable a = new();
        _ = a.AreSame(a).Should().BeTrue();
    }
}
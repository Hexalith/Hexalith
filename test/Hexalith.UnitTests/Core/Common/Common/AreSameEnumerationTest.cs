// <copyright file="AreSameEnumerationTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using FluentAssertions;

using Hexalith.Extensions.Helpers;

public class AreSameEnumerationTest
{
    [Fact]
    public void EquatableDictionaryAreSameShouldReturnFalse()
    {
        Dictionary<int, DummyEquatable> a = new()
        {
            [100] = new DummyEquatable(),
            [101] = new DummyEquatable() { Property3 = 10 },
        };
        Dictionary<int, DummyEquatable> b = new()
        {
            [100] = new DummyEquatable(),
            [101] = new DummyEquatable() { Property3 = 11 },
        };
        _ = a.AreSameDictionary(b).Should().BeFalse();
        _ = a.AreSame(b).Should().BeFalse();
    }

    [Fact]
    public void EquatableDictionaryAreSameShouldReturnTrue()
    {
        Dictionary<int, DummyEquatable> a = new()
        {
            [100] = new DummyEquatable(),
            [101] = new DummyEquatable() { Property3 = 10 },
        };
        Dictionary<int, DummyEquatable> b = new()
        {
            [100] = new DummyEquatable(),
            [101] = new DummyEquatable() { Property3 = 10 },
        };
        _ = a.AreSameDictionary(b).Should().BeTrue();
        _ = a.AreSame(b).Should().BeTrue();
    }

    [Fact]
    public void EquatableListAreSameShouldReturnFalse()
    {
        List<DummyEquatable> a = [new DummyEquatable(), new DummyEquatable() { Property3 = 10 }];
        List<DummyEquatable> b = [new DummyEquatable(), new DummyEquatable() { Property3 = 11 }];
        _ = a.AreSameEnumeration(b).Should().BeFalse();
        _ = a.AreSame(b).Should().BeFalse();
    }

    [Fact]
    public void EquatableListAreSameShouldReturnTrue()
    {
        List<DummyEquatable> a = [new DummyEquatable(), new DummyEquatable() { Property3 = 10 }];
        List<DummyEquatable> b = [new DummyEquatable(), new DummyEquatable() { Property3 = 10 }];
        _ = a.AreSameEnumeration(b).Should().BeTrue();
        _ = a.AreSame(b).Should().BeTrue();
    }

    [Fact]
    public void SimpleDictionaryAreSameShouldReturnFalse()
    {
        Dictionary<int, string> a = new() { [10] = "Hello", [11] = "world" };
        Dictionary<int, string> b = new() { [10] = "Hello", [11] = "world*" };
        _ = a.AreSameDictionary(b).Should().BeFalse();
        _ = a.AreSame(b).Should().BeFalse();
    }

    [Fact]
    public void SimpleDictionaryAreSameShouldReturnTrue()
    {
        Dictionary<int, string> a = new() { [10] = "Hello", [11] = "world" };
        Dictionary<int, string> b = new() { [10] = "Hello", [11] = "world" };
        _ = a.AreSameDictionary(b).Should().BeTrue();
        _ = a.AreSame(b).Should().BeTrue();
    }

    [Fact]
    public void SimpleListAreSameShouldReturnFalse()
    {
        List<string> a = ["Hello", "World"];
        List<string> b = ["Hello", "World*"];
        _ = a.AreSameEnumeration(b).Should().BeFalse();
        _ = a.AreSame(b).Should().BeFalse();
    }

    [Fact]
    public void SimpleListAreSameShouldReturnTrue()
    {
        List<string> a = ["Hello", "World"];
        List<string> b = ["Hello", "World"];
        _ = a.AreSameEnumeration(b).Should().BeTrue();
        _ = a.AreSame(b).Should().BeTrue();
    }
}
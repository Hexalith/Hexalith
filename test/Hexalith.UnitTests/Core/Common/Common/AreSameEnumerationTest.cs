// <copyright file="AreSameEnumerationTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using Shouldly;

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
        a.AreSameDictionary(b).ShouldBeFalse();
        a.AreSame(b).ShouldBeFalse();
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
        a.AreSameDictionary(b).ShouldBeTrue();
        a.AreSame(b).ShouldBeTrue();
    }

    [Fact]
    public void EquatableListAreSameShouldReturnFalse()
    {
        List<DummyEquatable> a = [new DummyEquatable(), new DummyEquatable() { Property3 = 10 }];
        List<DummyEquatable> b = [new DummyEquatable(), new DummyEquatable() { Property3 = 11 }];
        a.AreSameEnumeration(b).ShouldBeFalse();
        a.AreSame(b).ShouldBeFalse();
    }

    [Fact]
    public void EquatableListAreSameShouldReturnTrue()
    {
        List<DummyEquatable> a = [new DummyEquatable(), new DummyEquatable() { Property3 = 10 }];
        List<DummyEquatable> b = [new DummyEquatable(), new DummyEquatable() { Property3 = 10 }];
        a.AreSameEnumeration(b).ShouldBeTrue();
        a.AreSame(b).ShouldBeTrue();
    }

    [Fact]
    public void SimpleDictionaryAreSameShouldReturnFalse()
    {
        Dictionary<int, string> a = new() { [10] = "Hello", [11] = "world" };
        Dictionary<int, string> b = new() { [10] = "Hello", [11] = "world*" };
        a.AreSameDictionary(b).ShouldBeFalse();
        a.AreSame(b).ShouldBeFalse();
    }

    [Fact]
    public void SimpleDictionaryAreSameShouldReturnTrue()
    {
        Dictionary<int, string> a = new() { [10] = "Hello", [11] = "world" };
        Dictionary<int, string> b = new() { [10] = "Hello", [11] = "world" };
        a.AreSameDictionary(b).ShouldBeTrue();
        a.AreSame(b).ShouldBeTrue();
    }

    [Fact]
    public void SimpleListAreSameShouldReturnFalse()
    {
        List<string> a = ["Hello", "World"];
        List<string> b = ["Hello", "World*"];
        a.AreSameEnumeration(b).ShouldBeFalse();
        a.AreSame(b).ShouldBeFalse();
    }

    [Fact]
    public void SimpleListAreSameShouldReturnTrue()
    {
        List<string> a = ["Hello", "World"];
        List<string> b = ["Hello", "World"];
        a.AreSameEnumeration(b).ShouldBeTrue();
        a.AreSame(b).ShouldBeTrue();
    }
}
// <copyright file="Ordering.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.TopologicalSort;

using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions;

using Hexalith.Application.TopologicalSorting;

public class Ordering
{
    /// <summary>
    /// Test that a graph with a split in the middle will order properly.
    /// </summary>
    [Fact]
    public void BasicBranching()
    {
        DependencyGraph g = new();

        OrderedProcess a = new(g, "A");
        OrderedProcess b1 = new(g, "B1");
        OrderedProcess b2 = new(g, "B2");
        OrderedProcess c = new(g, "C");

        _ = a.Before(b1, b2).Before(c);

        IEnumerable<IEnumerable<OrderedProcess>> s = g.CalculateSort();

        _ = s.Skip(0).First().Count().Should().Be(1);
        _ = s.Skip(0).First().First().Should().Be(a);

        _ = s.Skip(1).First().Count().Should().Be(2);
        _ = s.Skip(1).First().Should().Contain(b1);
        _ = s.Skip(1).First().Should().Contain(b2);

        _ = s.Skip(2).First().Count().Should().Be(1);
        _ = s.Skip(2).First().First().Should().Be(c);
    }

    /// <summary>
    /// Test that a simple A->B->C graph works with every possible restriction on ordering.
    /// </summary>
    [Fact]
    public void BasicOrderAfter()
    {
        DependencyGraph g = new();

        OrderedProcess a = new(g, "A");
        OrderedProcess b = new(g, "B");
        OrderedProcess c = new(g, "C");

        _ = a.Before(b).Before(c);

        _ = c.After(b).After(a);

        IEnumerable<IEnumerable<OrderedProcess>> s = g.CalculateSort();

        _ = s.Skip(0).First().Count().Should().Be(1);
        _ = s.Skip(0).First().First().Should().Be(a);

        _ = s.Skip(1).First().Count().Should().Be(1);
        _ = s.Skip(1).First().First().Should().Be(b);

        _ = s.Skip(2).First().Count().Should().Be(1);
        _ = s.Skip(2).First().First().Should().Be(c);
    }

    /// <summary>
    /// Test that a simple A->B->C graph works with minimum restrictions on ordering.
    /// </summary>
    [Fact]
    public void BasicOrderBefore()
    {
        DependencyGraph g = new();

        OrderedProcess a = new(g, "A");
        OrderedProcess b = new(g, "B");
        OrderedProcess c = new(g, "C");

        _ = a.Before(b).Before(c);

        IEnumerable<IEnumerable<OrderedProcess>> s = g.CalculateSort();

        _ = s.Skip(0).First().Count().Should().Be(1);
        _ = s.Skip(0).First().First().Should().Be(a);

        _ = s.Skip(1).First().Count().Should().Be(1);
        _ = s.Skip(1).First().First().Should().Be(b);

        _ = s.Skip(2).First().Count().Should().Be(1);
        _ = s.Skip(2).First().First().Should().Be(c);
    }

    /// <summary>
    /// Tests that a complex branching system with an impossible constraint is detected.
    /// </summary>
    [Fact]
    public void BranchingUnorderable()
    {
        DependencyGraph g = new();

        OrderedProcess a = new(g, "A");
        OrderedProcess b1 = new(g, "B1");
        OrderedProcess b2 = new(g, "B2");
        OrderedProcess c1 = new(g, "C1");
        OrderedProcess c2 = new(g, "C2");
        OrderedProcess c3 = new(g, "C3");
        OrderedProcess c4 = new(g, "C4");
        OrderedProcess d = new(g, "D");

        _ = a.Before(b1, b2).Before(c1, c2, c3, c4).Before(d).Before(b1);

        Action calc = () => g.CalculateSort();
        _ = calc.Should().Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Test a complex branching scheme.
    /// </summary>
    [Fact]
    public void ComplexBranching()
    {
        DependencyGraph g = new();

        OrderedProcess a = new(g, "A");
        OrderedProcess b1 = new(g, "B1");
        OrderedProcess b2 = new(g, "B2");
        OrderedProcess c1 = new(g, "C1");
        OrderedProcess c2 = new(g, "C2");
        OrderedProcess c3 = new(g, "C3");
        OrderedProcess c4 = new(g, "C4");
        OrderedProcess d = new(g, "D");

        _ = a.Before(b1, b2).Before(c1, c2, c3, c4).Before(d);

        IEnumerable<IEnumerable<OrderedProcess>> s = g.CalculateSort();

        _ = s.Skip(0).First().Count().Should().Be(1);
        _ = s.Skip(0).First().First().Should().Be(a);

        _ = s.Skip(1).First().Count().Should().Be(2);
        _ = s.Skip(1).First().Should().Contain(b1);
        _ = s.Skip(1).First().Should().Contain(b2);

        _ = s.Skip(2).First().Count().Should().Be(4);
        _ = s.Skip(2).First().Should().Contain(c1);
        _ = s.Skip(2).First().Should().Contain(c2);
        _ = s.Skip(2).First().Should().Contain(c3);
        _ = s.Skip(2).First().Should().Contain(c4);

        _ = s.Skip(3).First().Count().Should().Be(1);
        _ = s.Skip(3).First().First().Should().Be(d);
    }

    /// <summary>
    /// Tests that an impossible ordering constraint is detected.
    /// </summary>
    [Fact]
    public void Unorderable()
    {
        DependencyGraph g = new();

        OrderedProcess a = new(g, "A");
        OrderedProcess b = new(g, "B");

        _ = a.Before(b);
        _ = b.Before(a);

        Action calc = () => g.CalculateSort();
        _ = calc.Should().Throw<InvalidOperationException>();
    }
}
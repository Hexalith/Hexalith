// <copyright file="Ordering.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.TopologicalSort;

using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

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

        s.Skip(0).First().Count().ShouldBe(1);
        s.Skip(0).First().First().ShouldBe(a);

        s.Skip(1).First().Count().ShouldBe(2);
        s.Skip(1).First().ShouldContain(b1);
        s.Skip(1).First().ShouldContain(b2);

        s.Skip(2).First().Count().ShouldBe(1);
        s.Skip(2).First().First().ShouldBe(c);
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

        s.Skip(0).First().Count().ShouldBe(1);
        s.Skip(0).First().First().ShouldBe(a);

        s.Skip(1).First().Count().ShouldBe(1);
        s.Skip(1).First().First().ShouldBe(b);

        s.Skip(2).First().Count().ShouldBe(1);
        s.Skip(2).First().First().ShouldBe(c);
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

        s.Skip(0).First().Count().ShouldBe(1);
        s.Skip(0).First().First().ShouldBe(a);

        s.Skip(1).First().Count().ShouldBe(1);
        s.Skip(1).First().First().ShouldBe(b);

        s.Skip(2).First().Count().ShouldBe(1);
        s.Skip(2).First().First().ShouldBe(c);
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
        Should.Throw<InvalidOperationException>(calc);
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

        s.Skip(0).First().Count().ShouldBe(1);
        s.Skip(0).First().First().ShouldBe(a);

        s.Skip(1).First().Count().ShouldBe(2);
        s.Skip(1).First().ShouldContain(b1);
        s.Skip(1).First().ShouldContain(b2);

        s.Skip(2).First().Count().ShouldBe(4);
        s.Skip(2).First().ShouldContain(c1);
        s.Skip(2).First().ShouldContain(c2);
        s.Skip(2).First().ShouldContain(c3);
        s.Skip(2).First().ShouldContain(c4);

        s.Skip(3).First().Count().ShouldBe(1);
        s.Skip(3).First().First().ShouldBe(d);
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
        Should.Throw<InvalidOperationException>(calc);
    }
}
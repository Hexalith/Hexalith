// <copyright file="Resources.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.TopologicalSort;

using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Hexalith.Application.TopologicalSorting;

public class Resources
{
    /// <summary>
    /// tests if basic resource resolution works.
    /// </summary>
    [Fact]
    public void BasicResourceResolution()
    {
        DependencyGraph g = new();

        Resource res = new(g, "resource");

        OrderedProcess a = new(g, "A");
        OrderedProcess b = new(g, "B");
        OrderedProcess c = new(g, "C");

        _ = a.Before(b);
        _ = a.Before(c);

        b.Requires(res);
        c.Requires(res);

        IEnumerable<IEnumerable<OrderedProcess>> s = g.CalculateSort();

        s.Count().ShouldBe(3);

        s.Skip(0).First().Count().ShouldBe(1);
        s.Skip(0).First().First().ShouldBe(a);

        s.Skip(1).First().Count().ShouldBe(1);
        (s.Skip(1).First().First() == b || s.Skip(1).First().First() == c).ShouldBeTrue();

        s.Skip(0).First().Count().ShouldBe(1);
        (s.Skip(2).First().First() == b || s.Skip(2).First().First() == c).ShouldBeTrue();

        s.Skip(1).First().First().ShouldNotBe(s.Skip(2).First().First());
    }

    /// <summary>
    /// Test if resource resolution works on a complex branching graph.
    /// </summary>
    [Fact]
    public void BranchingResourceResolution()
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

        Resource resource = new(g, "Resource");
        resource.UsedBy(c1, c3);

        IEnumerable<IEnumerable<OrderedProcess>> s = g.CalculateSort();

        // check that A comes first
        s.Skip(0).First().Count().ShouldBe(1);
        s.Skip(0).First().First().ShouldBe(a);

        // check that D comes last
        s.Skip(4).First().Count().ShouldBe(1);
        s.Skip(4).First().First().ShouldBe(d);

        // check that no set contains both c1 and c3
        s.Count(set => set.Contains(c1) && set.Contains(c3)).ShouldBe(0);
    }
}
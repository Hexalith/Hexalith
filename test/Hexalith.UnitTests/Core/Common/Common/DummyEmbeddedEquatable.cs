// <copyright file="DummyEmbeddedEquatable.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using System.Collections.Generic;

using Hexalith.Extensions.Common;

public class DummyEmbeddedEquatable : IEquatableObject
{
    public string Property1 { get; set; } = "Hi";

    public string Property2 { get; set; } = "Ho";

    public int Property3 { get; set; } = 1230;

    public DummyNonEquatable Property4 { get; set; } = new DummyNonEquatable();

    public DummyEquatable Property5 { get; set; } = new DummyEquatable();

    public IEnumerable<object> GetEqualityComponents()
                            => [Property1, Property2, Property3, Property4, Property5];
}
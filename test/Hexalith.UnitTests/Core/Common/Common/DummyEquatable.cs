// <copyright file="DummyEquatable.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Common;

using System.Collections.Generic;

using Hexalith.Extensions.Common;

public class DummyEquatable : IEquatableObject
{
    public string Property1 { get; set; } = "Prop1";

    public string Property2 { get; set; }

    public int Property3 { get; set; } = 123;

    public IEnumerable<object> GetEqualityComponents()
                    => [Property1, Property2, Property3];
}
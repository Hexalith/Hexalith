// <copyright file="ObjectToDictionnaryHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.Collections.Generic;

using FluentAssertions;

public class ObjectToDictionnaryHelper
{
    [Fact]
    public void Test()
    {
        // convert an object to a dictionnary
        var obj = new { Name = "John", Age = 42 };
        IDictionary<string, object> dico = ToDictionary(obj);

        _ = dico.Should().NotBeNull();
        _ = dico.Should().HaveCount(2);
        _ = dico.Should().ContainKey("Name");
        _ = dico.Should().ContainKey("Age");
        _ = dico["Name"].Should().Be("John");
        _ = dico["Age"].Should().Be(42);
    }

    private static IDictionary<string, object> ToDictionary(object obj)
    {
        Dictionary<string, object> dico = new(StringComparer.Ordinal);
        foreach (System.Reflection.PropertyInfo prop in obj.GetType().GetProperties())
        {
            dico.Add(prop.Name, prop.GetValue(obj));
        }

        return dico;
    }
}
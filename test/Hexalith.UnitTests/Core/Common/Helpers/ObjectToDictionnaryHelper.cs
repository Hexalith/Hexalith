// <copyright file="ObjectToDictionnaryHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.Collections.Generic;

using Shouldly;

public class ObjectToDictionnaryHelper
{
    [Fact]
    public void Test()
    {
        // convert an object to a dictionnary
        var obj = new { Name = "John", Age = 42 };
        IDictionary<string, object> dico = ToDictionary(obj);

        dico.ShouldNotBeNull();
        dico.Count.ShouldBe(2);
        dico.ShouldContainKey("Name");
        dico.ShouldContainKey("Age");
        dico["Name"].ShouldBe("John");
        dico["Age"].ShouldBe(42);
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
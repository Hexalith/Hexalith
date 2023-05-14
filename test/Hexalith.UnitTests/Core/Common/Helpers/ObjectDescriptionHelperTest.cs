// <copyright file="ObjectDescriptionHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using FluentAssertions;

using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Reflections;

public class ObjectDescriptionHelperTest
{
    [Fact]
    public void Mappable_type_object_should_return_mapper_value()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.DescribeType(typeof(MappableTypeTestV2));
        _ = type.Should().Be("MappableTest");
        _ = name.Should().Be("Mappable test");
        _ = description.Should().Be("Mappable test");
    }

    [Fact]
    public void Object_with_description_attribute_should_return_defined_value()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.DescribeType(typeof(DescriptionAttributeTest));
        _ = type.Should().Be(nameof(DescriptionAttributeTest));
        _ = name.Should().Be("Description attribute test");
        _ = description.Should().Be("This class is used to test a class with a description attribute");
    }

    [Fact]
    public void Object_with_display_attribute_should_return_defined_value()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.DescribeType(typeof(DisplayAttributeTest));
        _ = type.Should().Be(nameof(DisplayAttributeTest));
        _ = name.Should().Be("Display attribute example");
        _ = description.Should().Be("Example of using the display attribute to defined name and description");
    }

    [Fact]
    public void Object_with_display_name_attribute_should_return_defined_value()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.DescribeType(typeof(DisplayNameAttributeTest));
        _ = type.Should().Be(nameof(DisplayNameAttributeTest));
        _ = name.Should().Be("Display name attribute example");
        _ = description.Should().Be("Display name attribute example");
    }

    [Fact]
    public void Object_with_no_attributes_should_return_type_name()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.DescribeType(typeof(NoAttributesTest));
        _ = type.Should().Be(nameof(NoAttributesTest));
        _ = name.Should().Be("No attributes test");
        _ = description.Should().Be("No attributes test");
    }

    [Description("This class is used to test a class with a description attribute")]
    public class DescriptionAttributeTest
    {
    }

    [Display(Name = "Display attribute example", Description = "Example of using the display attribute to defined name and description")]
    public class DisplayAttributeTest
    {
    }

    [DisplayName("Display name attribute example")]
    public class DisplayNameAttributeTest
    {
    }

    public class MappableTypeTestV2 : IMappableType
    {
        public string TypeMapName => "MappableTest";
    }

    public class NoAttributesTest
    {
    }
}
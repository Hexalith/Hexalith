// <copyright file="ObjectDescriptionHelperTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Shouldly;

using Hexalith.Extensions.Helpers;
using Hexalith.Extensions.Reflections;

/// <summary>
/// Class ObjectDescriptionHelperTest.
/// </summary>
public class ObjectDescriptionHelperTest
{
    /// <summary>
    /// Defines the test method Mappable_type_object_should_return_mapper_value.
    /// </summary>
    [Fact]
    public void MappableTypeObjectShouldReturnMapperValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(MappableTypeTestV2));
        type.ShouldBe("MappableTest");
        name.ShouldBe("Mappable test");
        description.ShouldBe("Mappable test");
    }

    /// <summary>
    /// Defines the test method Object_with_description_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectWithDescriptionAttributeShouldReturnDefinedValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(DescriptionAttributeTest));
        type.ShouldBe(nameof(DescriptionAttributeTest));
        name.ShouldBe("Description attribute test");
        description.ShouldBe("This class is used to test a class with a description attribute");
    }

    /// <summary>
    /// Defines the test method Object_with_display_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectWithDisplayAttributeShouldReturnDefinedValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(DisplayAttributeTest));
        type.ShouldBe(nameof(DisplayAttributeTest));
        name.ShouldBe("Display attribute example");
        description.ShouldBe("Example of using the display attribute to defined name and description");
    }

    /// <summary>
    /// Defines the test method Object_with_display_name_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectWithDisplayNameAttributeShouldReturnDefinedValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(DisplayNameAttributeTest));
        type.ShouldBe(nameof(DisplayNameAttributeTest));
        name.ShouldBe("Display name attribute example");
        description.ShouldBe("Display name attribute example");
    }

    /// <summary>
    /// Defines the test method Object_with_no_attributes_should_return_type_name.
    /// </summary>
    [Fact]
    public void ObjectWithNoAttributesShouldReturnTypeName()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(NoAttributesTest));
        type.ShouldBe(nameof(NoAttributesTest));
        name.ShouldBe("No attributes test");
        description.ShouldBe("No attributes test");
    }

    /// <summary>
    /// Class DescriptionAttributeTest.
    /// </summary>
    [Description("This class is used to test a class with a description attribute")]
    public class DescriptionAttributeTest
    {
    }

    /// <summary>
    /// Class DisplayAttributeTest.
    /// </summary>
    [Display(Name = "Display attribute example", Description = "Example of using the display attribute to defined name and description")]
    public class DisplayAttributeTest
    {
    }

    /// <summary>
    /// Class DisplayNameAttributeTest.
    /// </summary>
    [DisplayName("Display name attribute example")]
    public class DisplayNameAttributeTest
    {
    }

    /// <summary>
    /// Class MappableTypeTestV2.
    /// Implements the <see cref="IMappableType" />.
    /// </summary>
    /// <seealso cref="IMappableType" />
    public class MappableTypeTestV2 : IMappableType
    {
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeMapName => "MappableTest";
    }

    /// <summary>
    /// Class NoAttributesTest.
    /// </summary>
    public class NoAttributesTest
    {
    }
}
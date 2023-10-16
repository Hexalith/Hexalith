// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 05-14-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-14-2023
// ***********************************************************************
// <copyright file="ObjectDescriptionHelperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Common.Helpers;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using FluentAssertions;

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
        _ = type.Should().Be("MappableTest");
        _ = name.Should().Be("Mappable test");
        _ = description.Should().Be("Mappable test");
    }

    /// <summary>
    /// Defines the test method Object_with_description_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectWithDescriptionAttributeShouldReturnDefinedValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(DescriptionAttributeTest));
        _ = type.Should().Be(nameof(DescriptionAttributeTest));
        _ = name.Should().Be("Description attribute test");
        _ = description.Should().Be("This class is used to test a class with a description attribute");
    }

    /// <summary>
    /// Defines the test method Object_with_display_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectWithDisplayAttributeShouldReturnDefinedValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(DisplayAttributeTest));
        _ = type.Should().Be(nameof(DisplayAttributeTest));
        _ = name.Should().Be("Display attribute example");
        _ = description.Should().Be("Example of using the display attribute to defined name and description");
    }

    /// <summary>
    /// Defines the test method Object_with_display_name_attribute_should_return_defined_value.
    /// </summary>
    [Fact]
    public void ObjectWithDisplayNameAttributeShouldReturnDefinedValue()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(DisplayNameAttributeTest));
        _ = type.Should().Be(nameof(DisplayNameAttributeTest));
        _ = name.Should().Be("Display name attribute example");
        _ = description.Should().Be("Display name attribute example");
    }

    /// <summary>
    /// Defines the test method Object_with_no_attributes_should_return_type_name.
    /// </summary>
    [Fact]
    public void ObjectWithNoAttributesShouldReturnTypeName()
    {
        (string type, string name, string description) = ObjectDescriptionHelper.Describe(typeof(NoAttributesTest));
        _ = type.Should().Be(nameof(NoAttributesTest));
        _ = name.Should().Be("No attributes test");
        _ = description.Should().Be("No attributes test");
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
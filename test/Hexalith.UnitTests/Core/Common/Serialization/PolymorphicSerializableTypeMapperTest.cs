// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 04-06-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-06-2023
// ***********************************************************************
// <copyright file="PolymorphicSerializableTypeMapperTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Common.Serialization;

using System.Collections.Generic;

using FluentAssertions;

using Hexalith.Extensions.Reflections;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class PolymorphicSerializableTypeMapperTest.
/// </summary>
public class PolymorphicSerializableTypeMapperTest
{
    /// <summary>
    /// Defines the test method Map_should_succeed.
    /// </summary>
    [Fact]
    public void MapShouldSucceed()
    {
        Dictionary<string, IPolymorphicSerializable> map = TypeMapper<IPolymorphicSerializable>.GetMap();
        _ = map.Should().NotBeNull();
    }

    /// <summary>
    /// Class TestMessage.
    /// Implements the <see cref="IPolymorphicSerializable" />.
    /// </summary>
    /// <seealso cref="IPolymorphicSerializable" />
    public class TestMessage : IPolymorphicSerializable
    {
        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <value>The major version.</value>
        public int MajorVersion { get; } = 1;

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <value>The minor version.</value>
        public int MinorVersion { get; } = 2;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; } = "Test";

        /// <inheritdoc/>
        public string TypeMapName => IPolymorphicSerializable.GetTypeMapName(TypeName, MajorVersion, MinorVersion);

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <value>The name of the type.</value>
        public string TypeName { get; } = nameof(TestMessage);
    }
}
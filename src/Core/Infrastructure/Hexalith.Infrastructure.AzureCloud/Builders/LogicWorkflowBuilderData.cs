// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.AzureCloud
// Author           : Jérôme Piquot
// Created          : 04-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-12-2023
// ***********************************************************************
// <copyright file="LogicWorkflowBuilderData.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.AzureCloud.Builders;

using System;

/// <summary>
/// Class LogicWorkflowData.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="LogicWorkflowBuilderData"/> class.
/// </remarks>
/// <param name="name">The name.</param>
/// <param name="location">The location.</param>
public abstract class LogicWorkflowBuilderData(string name, string? location)
{
    /// <summary>
    /// Gets the definition.
    /// </summary>
    /// <value>The definition.</value>
    public BinaryData Definition => BinaryData.FromString(GetDefinition());

    /// <summary>
    /// Gets or sets the location.
    /// </summary>
    /// <value>The location.</value>
    public string? Location { get; set; } = location;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string? Name { get; set; } = name;

    /// <summary>
    /// Gets the definition.
    /// </summary>
    /// <returns>System.String.</returns>
    protected abstract string GetDefinition();

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <returns>System.String.</returns>
    protected abstract string GetParameters();
}
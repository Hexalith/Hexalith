// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.SemanticBot
// Author           : Jérôme Piquot
// Created          : 05-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-08-2023
// ***********************************************************************
// <copyright file="ISemanticActivity.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.SemanticBot.Activities;

/// <summary>
/// Interface ISemanticActivity.
/// </summary>
public interface ISemanticActivity
{
    /// <summary>
    /// Gets the classification prompt.
    /// </summary>
    /// <value>The classification prompt.</value>
    string Classification { get; }

    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    /// <summary>
    /// Gets the prompt.
    /// </summary>
    /// <value>The prompt.</value>
    string Prompt { get; }

    /// <summary>
    /// Gets the get data prompt.
    /// </summary>
    /// <value>The get data prompt.</value>
    Type ResponseType { get; }
}
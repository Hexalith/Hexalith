// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.TeamsBot
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="OpenApiSkillsAuthHeaders.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.TeamsBot.Models;

using System;

using Hexalith.Application.ArtificialIntelligence;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Represents the authentication headers for imported OpenAPI Plugin Skills.
/// </summary>
public class OpenApiSkillsAuthHeaders
{
    /// <summary>
    /// Gets or sets the GitHub authentication header value.
    /// </summary>
    /// <value>The github authentication.</value>
    [FromHeader(Name = "x-sk-copilot-github-auth")]
    public string? GithubAuthentication { get; set; }

    /// <summary>
    /// Gets or sets the MS Graph authentication header value.
    /// </summary>
    /// <value>The graph authentication.</value>
    [FromHeader(Name = "x-sk-copilot-graph-auth")]
    public string? GraphAuthentication { get; set; }

    /// <summary>
    /// Gets or sets the Jira authentication header value.
    /// </summary>
    /// <value>The jira authentication.</value>
    [FromHeader(Name = "x-sk-copilot-jira-auth")]
    public string? JiraAuthentication { get; set; }

    /// <summary>
    /// Gets or sets the Klarna header value.
    /// </summary>
    /// <value>The klarna authentication.</value>
    [FromHeader(Name = "x-sk-copilot-klarna-auth")]
    public string? KlarnaAuthentication { get; set; }

    internal OpenApiAuthentication GetAuthentication() => throw new NotImplementedException();
}
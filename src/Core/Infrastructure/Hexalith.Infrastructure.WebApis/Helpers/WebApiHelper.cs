// <copyright file="WebApiHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.WebApis.Helpers;

using Hexalith.Application.Abstractions.Errors;

using Microsoft.AspNetCore.Mvc;

using SmartFormat;

using System.Globalization;
using System.Net;

/// <summary>
/// Class WebApiHelper.
/// </summary>
public static class WebApiHelper
{
    /// <summary>
    /// Converts an application error to a Mvc problem details object.
    /// </summary>
    /// <param name="problem">The problem.</param>
    /// <param name="technicalDetail">if set to <c>true</c> [add technical details].</param>
    /// <returns>ProblemDetails.</returns>
    public static ProblemDetails ToProblemDetails(this Error problem, bool technicalDetail)
    {
        string detail = technicalDetail
           ? Smart.Format(
               CultureInfo.InvariantCulture,
               problem.TechnicalDetail ?? string.Empty,
               problem.TechnicalArguments)
            : Smart.Format(
                CultureInfo.InvariantCulture,
                problem.Detail ?? string.Empty,
                problem.Arguments);
        return new()
        {
            Detail = detail,
            Instance = "https://github.com/Hexalith/Hexalith/issues/",
            Status = (int)HttpStatusCode.BadRequest,
            Title = problem.Title,
            Type = problem.Type,
        };
    }
}

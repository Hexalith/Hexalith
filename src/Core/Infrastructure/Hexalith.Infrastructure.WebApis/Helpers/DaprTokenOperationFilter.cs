// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.WebApis
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="DaprTokenOperationFilter.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.WebApis.Helpers;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

internal class DaprTokenOperationFilter : IOperationFilter
{
    /// <inheritdoc/>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters ??= [];

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "dapr-api-token",
            In = ParameterLocation.Header,
            Description = "Dapr api token",
            AllowEmptyValue = true,
            Schema = new OpenApiSchema
            {
                Type = "string",
            },
            Required = false,
        });
    }
}
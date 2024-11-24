// <copyright file="GlobalSuppressions.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Info Code Smell",
    "S1309:Track uses of in-source issue suppressions",
    Justification = "This is needed for new C# language support",
    Scope = "type",
    Target = "~T:Hexalith.Infrastructure.WebApis.ServicesRoutes")]
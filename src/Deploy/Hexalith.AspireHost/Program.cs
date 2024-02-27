// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.AspireHost.Helpers;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>();

builder.AddHexalithProject<Projects.Hexalith_Server_ExternalSystems>();

builder.AddHexalithProject<Projects.Hexalith_Server_FrontApi>();

builder.AddHexalithProject<Projects.Hexalith_Server_Parties>();

builder.AddHexalithProject<Projects.Hexalith_Server_Sales>();

builder.Build().Run();
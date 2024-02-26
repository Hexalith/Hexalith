// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.AspireHost.Helpers;

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>(
    "Hexalith.Server.Dynamics365Finance",
    1);

builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>(
    "Hexalith.Server.ExternalSystems",
    2);

builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>(
    "Hexalith.Server.FrontApi",
    3);

builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>(
    "Hexalith.Server.Parties",
    4);

builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>(
    "Hexalith.Server.Sales",
    5);

builder.Build().Run();
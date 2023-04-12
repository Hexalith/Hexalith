// ***********************************************************************
// Assembly         : Deploy
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-03-2023
// ***********************************************************************
// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

using Cocona;
using Cocona.Builder;

using DeployLCS.Configuration;

using Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;

CoconaAppBuilder builder = CoconaApp.CreateBuilder();
builder
    .Configuration
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<DeploymentCommand>()
    .AddEnvironmentVariables();
builder.Services.ConfigureSettings<LcsSettings>(builder.Configuration);
CoconaApp app = builder.Build();

app.AddCommands<DeploymentCommand>();

app.Run();
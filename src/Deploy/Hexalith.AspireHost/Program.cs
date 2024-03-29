// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

HexalithDistributedApplication app = new(args);

app.Builder.Configuration.AddUserSecrets<Program>();

if (app.IsProjectEnabled<Projects.Hexalith_Server_ExternalSystems>())
{
    _ = app.AddProject<Projects.Hexalith_Server_ExternalSystems>("externalsystems");
}

if (app.IsProjectEnabled<Projects.Hexalith_Server_FrontApi>())
{
    _ = app.AddProject<Projects.Hexalith_Server_FrontApi>("front-api");
}

if (app.IsProjectEnabled<Projects.Hexalith_Server_Parties>())
{
    _ = app.AddProject<Projects.Hexalith_Server_Parties>("parties");
}

if (app.IsProjectEnabled<Projects.Hexalith_Server_Sales>())
{
    _ = app.AddProject<Projects.Hexalith_Server_Sales>("sales");
}

if (app.IsProjectEnabled<Projects.Hexalith_Server_Inventories>())
{
    _ = app.AddProject<Projects.Hexalith_Server_Inventories>("inventories");
}
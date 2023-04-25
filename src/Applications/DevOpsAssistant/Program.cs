// ***********************************************************************
// Assembly         : DevOpsAssistant
// Author           : Jérôme Piquot
// Created          : 04-02-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-02-2023
// ***********************************************************************
// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

using DevOpsAssistant.Application.Configuration;
using DevOpsAssistant.Infrastructure.Actors;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Commands;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.WebApis.Helpers;

using Serilog;

const string appName = "DevOps Assistant";

#if DEBUG
bool debugInVisualStudio = true;
#else
bool debugInVisualStudio = false;
#endif

WebApplicationBuilder builder = HexalithWebApi.CreateApplication(
    appName,
    "v1",
    debugInVisualStudio,
    (actors) => actors.RegisterActor<DevOpsUnitOfWorkAggregateActor>(),
    args);

builder.Services.AddSingleton<ICommandDispatcher, DependencyInjectionCommandDispatcher>();
builder.Services.ConfigureSettings<DevOpsUnitOfWorkSettings>(builder.Configuration);

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCloudEvents();

app.MapControllers();

app.MapSubscribeHandler();

if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseRouting();

app.MapActorsHandlers();

Log.Logger.Information("Starting {AppName}.", appName);

try
{
    await app.RunAsync().ConfigureAwait(false);
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "Error starting {AppName}.", appName);
    throw;
}
finally
{
    Log.Logger.Information("{AppName}, is stopped.", appName);
}
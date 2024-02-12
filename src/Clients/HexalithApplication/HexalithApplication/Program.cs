// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.Infrastructure.ClientAppOnServer.Helpers;

using Serilog;

const string appName = "Hexalith Application";

#if DEBUG
bool debugInVisualStudio = true;
#else
bool debugInVisualStudio = false;
#endif

WebApplicationBuilder builder = ServerSideClientAppHelper.CreateServerSideClientApplication(
    appName,
    $".{nameof(Hexalith)}.Application",
    "v1",
    debugInVisualStudio,
    (actors) => { },
    args);

builder.Services
    .AddHexalithServerSideClientApp(
        builder.Configuration,
        typeof(HexalithApplication.Client._Imports).Assembly);

WebApplication app = builder.Build();
app.UseHexalithWebApplication();

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
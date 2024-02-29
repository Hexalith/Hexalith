// <copyright file="Program.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

IDistributedApplicationBuilder builder = DistributedApplication
    .CreateBuilder(args)
    .AddDapr();

IResourceBuilder<Aspire.Hosting.Dapr.IDaprComponentResource> events = builder
    .AddDaprPubSub("event-bus");
IResourceBuilder<Aspire.Hosting.Dapr.IDaprComponentResource> commands = builder
    .AddDaprPubSub("command-bus");
IResourceBuilder<Aspire.Hosting.Dapr.IDaprComponentResource> requests = builder
    .AddDaprPubSub("request-bus");
IResourceBuilder<Aspire.Hosting.Dapr.IDaprComponentResource> notifications = builder
    .AddDaprPubSub("notification-bus");
IResourceBuilder<Aspire.Hosting.Dapr.IDaprComponentResource> partiesStatestore = builder
    .AddDaprStateStore("parties-statestore");

// builder.AddHexalithProject<Projects.Hexalith_Server_Dynamics365Finance>();

// builder.AddHexalithProject<Projects.Hexalith_Server_ExternalSystems>();

// builder.AddHexalithProject<Projects.Hexalith_Server_FrontApi>();
builder.AddProject<Projects.Hexalith_Server_Parties>("parties")
    .WithDaprSidecar()
    .WithReference(events)
    .WithReference(commands)
    .WithReference(requests)
    .WithReference(notifications)
    .WithReference(partiesStatestore);

// builder.AddHexalithProject<Projects.Hexalith_Server_Sales>();
builder.Build().Run();
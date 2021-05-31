namespace Hexalith.Emails
{
    using Hexalith.Application.Commands;
    using Hexalith.Application.Events;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.Emails.Application.CommandHandlers;
    using Hexalith.Emails.Application.Commands;
    using Hexalith.Emails.Application.EventHandlers;
    using Hexalith.Emails.Application.Queries;
    using Hexalith.Emails.Application.Services;
    using Hexalith.Emails.Application.Settings;
    using Hexalith.Emails.Contracts.Events;
    using Hexalith.Emails.Domain.States;
    using Hexalith.Infrastructure.EfCore.Repositories;
    using Hexalith.Infrastructure.Helpers;
    using Hexalith.Infrastructure.WebServer.Modules;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using System;

    public sealed class EmailsServerModule : ServerModule
    {
        private EmailsSettings? _settings;

        public EmailsServerModule(IConfiguration configuration, IWebHostEnvironment environment) : base(configuration, environment)
        {
        }

        public EmailsSettings Settings => _settings ??= Configuration.GetSettings<EmailsSettings>();

        public override void ConfigureMessages(IMessageFactoryBuilder messageBuilder)
        {
            _ = messageBuilder ?? throw new ArgumentNullException(nameof(messageBuilder));
            messageBuilder.AddAssemblyMessages(typeof(GetEmailDetails).Assembly);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.ConfigureSettings<EmailsSettings>(Configuration);
            services.AddTransient<IRepository<IEmailState>, EfRepository<IEmailState, EmailState>>();
            services.AddHostedService<ReceiveAllEmailsJob>();
            services.AddHostedService<ReceiveUnreadEmailsJob>();
            services.AddTransient<ICommandHandler<ReceiveEmail>, ReceiveEmailHandler>();
            services.AddTransient<ICommandHandler<ReceiveAllEmails>, ReceiveAllEmailsHandler>();
            services.AddTransient<ICommandHandler<ReceiveUnreadEmails>, ReceiveUnreadEmailsHandler>();
            services.AddTransient<IEventHandler<EmailReceived>, EmailReceivedHandler>();

            // TODO move to graph module
            services.AddTransient<IMailboxService, GraphMailboxService>();
        }
    }
}
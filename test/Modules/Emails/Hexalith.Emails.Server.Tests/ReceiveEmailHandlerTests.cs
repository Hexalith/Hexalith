namespace Hexalith.Emails.Server.Tests
{
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.Emails.Application.CommandHandlers;
    using Hexalith.Emails.Application.Commands;
    using Hexalith.Emails.Contracts.Events;
    using Hexalith.Emails.Contracts.ValueTypes;
    using Hexalith.Emails.Domain.States;

    using Microsoft.Extensions.Logging;

    using Moq;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class ReceiveEmailHandlerTests
    {
        [Fact]
        public async Task Handle_publishes_EmailReceived_event()
        {
            var loggerMock = new Mock<ILogger<ReceiveEmailHandler>>();
            var receive = CreateReceiveEmail();
            var mockRepository = new Mock<IRepository<IEmailState>>();
            mockRepository
                .Setup(x => x.Save(
                    It.IsAny<CancellationToken>()
                ))
                .Returns(Task.CompletedTask);
            mockRepository
                .Setup(x => x.AddStateLog(
                    It.IsAny<string>(),
                    It.IsAny<IRepositoryMetadata>(),
                    It.IsAny<IEnumerable<object>>(),
                    It.IsAny<CancellationToken>()
                ))
                .Returns(Task.CompletedTask);
            mockRepository
                .Setup(x => x.Publish(
                    It.IsAny<IEnumerable<IEnvelope>>(),
                    It.IsAny<CancellationToken>()
                ))
                .Returns(Task.CompletedTask);
            mockRepository
                .Setup(x => x.SetState(
                    It.IsAny<string>(),
                    It.IsAny<IRepositoryMetadata>(),
                    It.IsAny<IEmailState>(),
                    It.IsAny<CancellationToken>()
                ))
                .Returns(Task.CompletedTask);
            var repository = mockRepository.Object;
            var handler = new ReceiveEmailHandler(repository, loggerMock.Object);
            await handler.Handle(new Envelope<ReceiveEmail>(
                receive,
                "msgid123",
                "test user",
                DateTimeOffset.Now
                ));
            mockRepository.Verify(x => x.Publish(
                    It.Is<IEnumerable<IEnvelope>>(p =>
                        p.Count() == 1 &&
                        p.First().Message.GetType() == typeof(EmailReceived)
                    ),
                    It.IsAny<CancellationToken>()
                ));
            mockRepository.Verify(x => x.AddStateLog(
                    It.Is<string>(p => p == receive.EmailId),
                    It.Is<IRepositoryMetadata>(p => p.MessageId == "msgid123" && p.UserName == "test user"),
                    It.Is<IEnumerable<IEnvelope>>(p =>
                        p.Count() == 1 &&
                        p.First().Message.GetType() == typeof(EmailReceived)
                    ),
                    It.IsAny<CancellationToken>()
                ));
            mockRepository.Verify(x => x.SetState(
                    It.Is<string>(p => p == receive.EmailId),
                    It.Is<IRepositoryMetadata>(p => p.MessageId == "msgid123" && p.UserName == "test user"),
                    It.Is<IRepositoryData<IEmailState>>(p =>
                        p.State.CopyToRecipients.Count == receive.CopyToRecipients.Count() &&
                        p.State.ToRecipients.Count == receive.ToRecipients.Count() &&
                        p.State.Attachments.Count == receive.Attachments.Count() &&
                        p.State.Body == receive.Body &&
                        p.State.Recipient == receive.Recipient &&
                        p.State.Subject == receive.Subject &&
                        p.State.Sender == receive.Sender &&
                        p.Events.Count() == 1 &&
                        p.Events.First().GetType() == typeof(EmailReceived) &&
                        ((EmailReceived)p.Events.First()).EmailId == receive.EmailId &&
                        ((EmailReceived)p.Events.First()).Body == receive.Body &&
                        ((EmailReceived)p.Events.First()).Sender == receive.Sender &&
                        ((EmailReceived)p.Events.First()).Recipient == receive.Recipient &&
                        ((EmailReceived)p.Events.First()).Subject == receive.Subject &&
                        ((EmailReceived)p.Events.First()).CopyToRecipients.Count() == receive.CopyToRecipients.Count() &&
                        ((EmailReceived)p.Events.First()).ToRecipients.Count() == receive.ToRecipients.Count() &&
                        ((EmailReceived)p.Events.First()).Attachments.Count() == receive.Attachments.Count()
                ), It.IsAny<CancellationToken>()),
                Times.Once);
            mockRepository.Verify(x => x.Save(
                    It.IsAny<CancellationToken>()
                ));
        }

        private static ReceiveEmail CreateReceiveEmail()
        {
            var attachments = new Attachment[]
            {
                new (){Name = "File1", Content = "ABCD==" },
                new (){Name = "File2", Content = "FFEE==" },
                new (){Name = "File3", Content = "123456789==" }
            };
            return new ReceiveEmail
            {
                EmailId = "Email123",
                Sender = "toto@titi.net",
                Attachments = attachments,
                Body = "Hello world!",
                CopyToRecipients = new[] { "mail1@tot.com", "mail2@titi.com" },
                Recipient = "reci@dada.com",
                Subject = "I am testing",
                ToRecipients = new[] { "ggg@hello.com", "ggghh@nan.info", "reci@dada.com" }
            };
        }
    }
}
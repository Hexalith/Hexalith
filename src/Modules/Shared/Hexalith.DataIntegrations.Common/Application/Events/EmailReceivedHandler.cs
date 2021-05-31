namespace Hexalith.DataIntegrations.Application.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Events;
    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Helpers;
    using Hexalith.Application.Messages;
    using Hexalith.Application.Repositories;
    using Hexalith.DataIntegrations.Application.Commands;
    using Hexalith.DataIntegrations.Common.Domain.ValueTypes;
    using Hexalith.DataIntegrations.Domain;
    using Hexalith.DataIntegrations.Domain.States;
    using Hexalith.Domain.ValueTypes;
    using Hexalith.Emails.Contracts.Events;

    using Microsoft.Extensions.Logging;

    using SharpCompress.Archives.Zip;

    [EventHandler(Event = typeof(EmailReceived))]
    public class EmailReceivedHandler : IEventHandler<EmailReceived>
    {
        private readonly ILogger<EmailReceivedHandler> _logger;
        private readonly IRepository<IDataIntegrationState> _repository;

        public EmailReceivedHandler(IRepository<IDataIntegrationState> repository, ILogger<EmailReceivedHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public Task Handle(IEnvelope envelope, CancellationToken cancellationToken = default)
           => Handle(new Envelope<EmailReceived>(envelope), cancellationToken);

        public async Task Handle(Envelope<EmailReceived> envelope, CancellationToken cancellationToken = default)
        {
            try
            {
                foreach (var submission in GetAttachments(envelope))
                {
                    var state = new DataIntegrationState();
                    var integration = new DataIntegration(submission.DataIntegrationId, state);
                    var events = await integration.Submit(
                                name: submission.Name,
                                description: submission.Description,
                                documentName: submission.DocumentName,
                                documentType: submission.DocumentType,
                                document: submission.Document);

                    await _repository.SetState(submission.DataIntegrationId, envelope.ToMetadata(), state, cancellationToken);
                    await _repository.Publish(events
                        .Select(p => new Envelope(p, new MessageId(), envelope))
                        .ToList(), cancellationToken);
                }
                await _repository.Save(cancellationToken);
            }
            catch (DuplicateRepositoryStateException)
            {
                _logger.LogError($"Duplicate integration submission : Id='{envelope.Message.EmailId}', Name='{envelope.Message.Subject}', MessageId='{envelope.MessageId}'.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Data integration email received event handler error : {e.Message}.\nMessageId={envelope.MessageId}; EmailId={envelope.Message.EmailId}; Subject={envelope.Message.Subject}");
                throw;
            }
        }

        private static List<SubmitDataIntegration> GetAttachments(Envelope<EmailReceived> envelope)
        {
            List<SubmitDataIntegration> list = new();
            var description = $"Mailbox : {envelope.Message.Recipient}\nFrom : {envelope.Message.Sender}\nBody :\n{envelope.Message.Body}";
            foreach (var attachment in envelope.Message.Attachments)
            {
                string? fileExt = Path.GetExtension(attachment.Name.ToUpperInvariant());
                if (fileExt == ".ZIP")
                {
                    using MemoryStream stream = new(Convert.FromBase64String(attachment.Content));
                    using var zip = ZipArchive.Open(stream);
                    foreach (var entry in zip.Entries.Where(p => p.IsDirectory == false))
                    {
                        using var entryStream = entry.OpenEntryStream();
                        using MemoryStream content = new();
                        entryStream.CopyTo(content);
                        content.Flush();
                        content.Position = 0;
                        string base64Content = Convert.ToBase64String(content.GetBuffer(), 0, (int)content.Length);
                        // TODO file name
                        var message = GetSubmitMessage(envelope,
                                                       entry.Key,
                                                       description,
                                                       base64Content);
                        if (message == null)
                        {
                            continue;
                        }
                        list.Add(message);
                    }
                }
                else
                {
                    var message = GetSubmitMessage(envelope, attachment.Name, description, attachment.Content);
                    if (message == null)
                    {
                        continue;
                    }
                    list.Add(message);
                }
            }
            return list;
        }

        private static SubmitDataIntegration? GetSubmitMessage(Envelope<EmailReceived> envelope, string name, string description, string content)
        {
            string? fileType = Path.GetExtension(name.ToUpperInvariant())
            switch
            {
                ".CSV" => nameof(FileType.Csv),
                ".TXT" => nameof(FileType.Csv),
                ".XML" => nameof(FileType.Xml),
                ".XLS" => nameof(FileType.Xls),
                ".XLSX" => nameof(FileType.Xlsx),
                _ => null
            };
            return (fileType == null) ? null : new()
            {
                DataIntegrationId = envelope.Message.EmailId + "/" + name,
                Name = envelope.Message.Subject + " - " + name,
                Description = description,
                DocumentName = name,
                DocumentType = fileType,
                Document = content
            };
        }
    }
}
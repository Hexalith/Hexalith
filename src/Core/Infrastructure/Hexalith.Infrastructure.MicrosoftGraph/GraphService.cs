namespace Hexalith.Infrastructure.MicrosoftGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Graph;

    public class GraphService
    {
        private GraphServiceClient? _graphClient;

        public GraphService(GraphAuthenticationService authenticationService)
        {
            AuthenticationService = authenticationService;
        }

        protected GraphAuthenticationService AuthenticationService { get; }
        protected GraphServiceClient GraphClient => _graphClient ??= InitializeGraphClient();

        public static IReadOnlyList<Attachment> GetAttachments(IMessageAttachmentsCollectionPage? attachments, CancellationToken cancellationToken = default)
        {
            if (attachments == null || attachments.Count == 0)
            {
                return Array.Empty<Attachment>();
            }
            List<Attachment> files = new(attachments.ToArray());
            while (attachments.NextPageRequest != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                attachments = attachments
                    .NextPageRequest
                    .GetAsync(cancellationToken)
                    .GetAwaiter()

                    .GetResult();
                files.AddRange(attachments.ToList());
            }
            return files;
        }

        public static IReadOnlyList<FileAttachment> GetFileAttachments(IMessageAttachmentsCollectionPage? attachments, CancellationToken cancellationToken = default)
        {
            if (attachments == null || attachments.Count == 0)
            {
                return Array.Empty<FileAttachment>();
            }
            List<FileAttachment> files = new(attachments.OfType<FileAttachment>().ToArray());
            while (attachments.NextPageRequest != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                attachments = attachments
                    .NextPageRequest
                    .GetAsync(cancellationToken)
                    .GetAwaiter()
                    .GetResult();
                files.AddRange(attachments.OfType<FileAttachment>().ToList());
            }
            return files;
        }

        public async Task<IEnumerable<string>> GetIdFromInternetMessageId(string recipient, string emailId, CancellationToken cancellationToken = default)
            => (await GraphClient
                .Users[recipient]
                .Messages
                .Request()
                .Filter($"{nameof(Message.InternetMessageId)} eq '{emailId}'")
                .Select(nameof(Message.Id))
                .GetAsync(cancellationToken)
                .ConfigureAwait(false))
                    .Select(p => p.Id)
                    .ToList();

        public async Task<IEnumerable<string>> GetUserIds(CancellationToken cancellationToken = default)
        {
            var users = await GraphClient
                .Users
                .Request()
                .GetAsync(cancellationToken)
                .ConfigureAwait(false);
            List<string> ids = new(users.Select(p => p.UserPrincipalName));
            while (users.NextPageRequest != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                users = await users
                    .NextPageRequest
                    .GetAsync(cancellationToken)
                    .ConfigureAwait(false);
                ids.AddRange(users.Select(p => p.UserPrincipalName).ToArray());
            }
            return ids;
        }

        public async Task<IEnumerable<Message>> GetUserMails(string userPrincipalName, bool unreadOnly = false, CancellationToken cancellationToken = default)
        {
            IUserMessagesCollectionPage? messages;
            if (unreadOnly)
            {
                messages = await GraphClient
                    .Users[userPrincipalName]
                    .Messages
                    .Request()
                    .Filter($"{nameof(Message.IsRead)} ne true")
                    .Expand(p => p.Attachments)
                    .GetAsync(cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                messages = await GraphClient
                    .Users[userPrincipalName]
                    .Messages
                    .Request()
                    .Expand(p => p.Attachments)
                    .GetAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            List<Message> ids = new(messages.ToList());
            while (messages.NextPageRequest != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                messages = await messages
                    .NextPageRequest
                    .GetAsync(cancellationToken)
                    .ConfigureAwait(false);
                ids.AddRange(messages.ToList());
            }
            return ids;
        }

        public async Task SetEmailAsRead(string userPrincipalName, string emailId, CancellationToken cancellationToken = default)
        {
            foreach (var id in await GetIdFromInternetMessageId(userPrincipalName, emailId, cancellationToken).ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();
                await GraphClient
                    .Users[userPrincipalName]
                    .Messages[id]
                    .Request()
                    .UpdateAsync(new Message()
                    {
                        IsRead = true
                    }, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private GraphServiceClient InitializeGraphClient()
                    => new(AuthenticationService.AuthenticationProvider);
    }
}
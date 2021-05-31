using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Emails.Application.Commands;

namespace Hexalith.Emails.Application.Services
{
    public interface IMailboxService
    {
        Task<IEnumerable<string>> GetUserIds(CancellationToken cancellationToken = default);

        Task<IEnumerable<ReceiveEmail>> GetUserMails(string userPrincipalName, bool unreadOnly = false, CancellationToken cancellationToken = default);

        Task SetEmailAsRead(string userPrincipalName, string emailId, CancellationToken cancellationToken = default);
    }
}
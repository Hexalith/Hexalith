namespace Hexalith.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public interface IIdCollectionService
{
    Task AddAsync(string aggregateGlobalId, CancellationToken cancellationToken);
}

using System.Collections.Generic;

using Hexalith.Application.Queries;

namespace Hexalith.Users.Application.Queries
{
    [ApiQuery(typeof(List<int>))]
    public sealed record GetAllUserIds
    {
        public GetAllUserIds() { }
        public GetAllUserIds(int take, int skip)
            => (Take, Skip) = (take, skip);

        public int Skip { get; }
        public int Take { get; }
    }
}
namespace Hexalith.Users.Application.Queries
{
    using System.Collections.Generic;

    using Hexalith.Application.Queries;

    public sealed class GetUsersearchMatching : QueryBase<List<int>>
    {
        public GetUsersearchMatching(string pattern, int take = 0, int skip = 0)
            => (Pattern, Take, Skip) = (pattern, take, skip);

        public string Pattern { get; }
        public int Skip { get; }
        public int Take { get; }
    }
}
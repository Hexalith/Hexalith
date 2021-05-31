namespace Bistrotic.Users.Application.Queries
{
    using System.Collections.Generic;

    using Bistrotic.Application.Queries;

    public sealed class GetUsersearchMatching : QueryBase<List<int>>
    {
        public GetUsersearchMatching(string pattern, int take = 0, int skip = 0)
        {
            Pattern = pattern;
            Take = take;
            Skip = skip;
        }

        public string Pattern { get; }
        public int Skip { get; }
        public int Take { get; }
    }
}
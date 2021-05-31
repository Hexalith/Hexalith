namespace Hexalith.Units.Application.Queries
{
    using System.Collections.Generic;

    using Hexalith.Application.Queries;

    public sealed class GetUnitSearchMatching : QueryBase<List<int>>
    {
        public GetUnitSearchMatching(string pattern, int take = 0, int skip = 0)
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
namespace Hexalith.WorkItems.Application.Queries
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Application.Queries;
    using Hexalith.WorkItems.Application.ModelViews;

    public sealed class GetSecurityGroupMembers : QueryBase<IEnumerable<SecurityGroupMember>>
    {
        public GetSecurityGroupMembers(string groupName)
        {
            GroupName = groupName;
        }

        [Obsolete("For serialization only")]
        public GetSecurityGroupMembers()
        {
            GroupName = string.Empty;
        }

        public string GroupName { get; }
    }
}
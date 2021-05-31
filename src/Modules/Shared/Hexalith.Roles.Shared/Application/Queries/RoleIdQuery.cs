namespace Hexalith.Roles.Application.Queries
{
    using Hexalith.Application.Queries;
    using Hexalith.Roles.Domain.ValueTypes;

    public abstract class RoleIdQuery<TResult> : QueryBase<RoleId, TResult>
    {
        protected RoleIdQuery()
        {
        }

        protected RoleIdQuery(RoleId unitId)
            : base(unitId)
        {
        }
    }
}
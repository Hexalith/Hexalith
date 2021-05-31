namespace Hexalith.Roles.Domain.Events
{
    using Hexalith.Domain.Messages;
    using Hexalith.Roles.Domain.ValueTypes;

    public abstract class RoleIdEvent : EventBase<RoleId>
    {
        protected RoleIdEvent(RoleId unitId)
            : base(unitId)
        {
        }
    }
}
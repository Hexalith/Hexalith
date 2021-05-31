namespace Hexalith.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Hexalith.Infrastructure.Client;
    public class MenuService : IMenuService
    {
        public IReadOnlyList<MenuItemDefinition> MenuItems =>
            new MenuItemDefinition[]
            {
                new MenuItemDefinition("Issues", "issues/sla/waitingaction", "Pending SLA issues", "oi oi-list-rich"),
                new MenuItemDefinition(true),
                new MenuItemDefinition("Swagger", "swagger", "API description page", "oi oi-list-rich")
            };
    }
}

namespace Hexalith.Infrastructure.Client
{
    using System.Collections.Generic;

    public class MenuItemDefinition
    {
        public MenuItemDefinition(string name, string link, string? description = null, string? icon = null)
        {
            Name = name;
            Link = link;
            Description = description;
            Icon = icon;
        }

        public MenuItemDefinition(bool isSeparator)
        {
            Name = Link = string.Empty;
            IsSeparator = isSeparator;
        }

        public MenuItemDefinition(string name, IReadOnlyList<MenuItemDefinition> subMenuItems, string? description = null, string? icon = null)
           : this(name, string.Empty, description, icon)
        {
            SubMenuItems = subMenuItems;
        }

        public string? Description { get; }

        public string? Icon { get; }

        public bool IsSeparator { get; }

        public string Link { get; }

        public string Name { get; }

        public IReadOnlyList<MenuItemDefinition>? SubMenuItems { get; }
    }
}
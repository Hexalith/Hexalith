namespace Hexalith.ApplicationLayer.Domain
{
    using System.Collections.Generic;

    using Hexalith.ApplicationLayer.Application.Events;
    using Hexalith.Domain.Exceptions;

    internal class ThemeSystemState
    {
        public Dictionary<int, string> Scripts { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> Stylesheets { get; set; } = new Dictionary<int, string>();

        public void Apply(List<object> events)
        {
            foreach (var @event in events)
            {
                _ = @event switch
                {
                    ScriptAddedToThemeSystem e => ThemeName = e.NewThemeName,
                    _ => throw new EventNotSupportedException<IUserSettingsState>(@event.GetType())
                };
            }
        }
    }
}
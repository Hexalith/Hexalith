namespace Hexalith.ApplicationLayer.Domain
{
    using System.Collections.Generic;

    using Hexalith.ApplicationLayer.Application.Events;
    using Hexalith.Domain.Exceptions;

    public class UserSettingsState : IUserSettingsState
    {
        public string ThemeName { get; set; } = string.Empty;

        public void Apply(List<object> events)
        {
            foreach (var @event in events)
            {
                _ = @event switch
                {
                    UserInterfaceThemeChanged e => ThemeName = e.NewThemeName,
                    _ => throw new EventNotSupportedException<IUserSettingsState>(@event.GetType())
                };
            }
        }
    }
}
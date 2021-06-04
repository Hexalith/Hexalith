namespace Hexalith.ApplicationLayer.Domain
{
    using System.Collections.Generic;

    using Hexalith.ApplicationLayer.Application.Events;

    internal class UserSettings
    {
        private readonly string _userName;
        private IUserSettingsState _state;

        public UserSettings(string userName, IUserSettingsState state)
        {
            _userName = userName;
            _state = state;
        }

        public IEnumerable<object> ChangeUserInterfaceTheme(string themeName)
        {
            List<object> events = new(new object[] { new UserInterfaceThemeChanged { UserName = _userName, OldThemeName = _state.ThemeName, NewThemeName = themeName } });
            _state.Apply(events);
            return events;
        }
    }
}
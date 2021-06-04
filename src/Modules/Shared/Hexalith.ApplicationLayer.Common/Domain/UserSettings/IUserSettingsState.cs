namespace Hexalith.ApplicationLayer.Domain
{
    using System.Collections.Generic;

    internal interface IUserSettingsState
    {
        string ThemeName { get; set; }

        void Apply(List<object> events);
    }
}
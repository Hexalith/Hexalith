using System.Collections.Generic;

namespace Hexalith.ApplicationLayer.Domain
{
    internal interface IUserSettingsState
    {
        string ThemeName { get; set; }

        void Apply(List<object> events);
    }
}
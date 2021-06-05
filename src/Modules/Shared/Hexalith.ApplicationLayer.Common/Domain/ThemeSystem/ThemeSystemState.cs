namespace Hexalith.ApplicationLayer.Domain
{
    using System.Collections.Generic;

    using Hexalith.ApplicationLayer.Events;
    using Hexalith.Domain.Exceptions;

    internal class ThemeSystemState
    {
        public Dictionary<int, string> ScriptIndex { get; set; } = new Dictionary<int, string>();
        public List<Script> Scripts { get; set; } = new Dictionary<int, string>();
        public Dictionary<int, string> StylesheetIndex { get; set; } = new Dictionary<int, string>();
        public List<Stylesheet> Stylesheets { get; set; } = new Dictionary<int, string>();

        public void Apply(List<object> events)
        {
            foreach (var @event in events)
            {
                switch (@event)
                {
                    case ScriptAddedToThemeSystem e:
                        Scripts.Add(e.Position, e.Script);
                        break;

                    case StylesheetAddedToThemeSystem e:
                        Stylesheets.Add(e.Position, e.Stylesheet);
                        break;

                    case ScriptMovedInThemeSystem e:
                        Scripts.Remove(e.FromPosition);
                        Scripts.Add(e.ToPosition, e.Script);
                        break;

                    case StylesheetMovedInThemeSystem e:
                        Stylesheets.Remove(e.FromPosition);
                        Stylesheets.Add(e.ToPosition, e.Stylesheet);
                        break;

                    case ScriptRemovedFromThemeSystem e:
                        Scripts.Remove(e.Position);
                        break;

                    case StylesheetRemovedFromThemeSystem e:
                        Stylesheets.Remove(e.Position);
                        break;

                    default:
                        throw new EventNotSupportedException<IUserSettingsState>(@event.GetType());
                }
            }
        }
    }
}
namespace Hexalith.Application.Abstractions.Tests.Fixtures
{
    using System.Collections.Generic;

    using Hexalith.Application.Repositories;

    public class DummyState : IEventDrivenState, IDummyState
    {
        public bool Disabled { get; set; }
        public bool Value1 { get; set; }
        public int Value2 { get; set; }
        public string Value3 { get; set; }

        public void Apply(IEnumerable<object> events)
        {
            foreach (var msg in events)
            {
                switch (msg)
                {
                    case NewDummyAdded d:
                        Value1 = d.Value1;
                        Value2 = d.Value2;
                        Value3 = d.Value3;
                        break;

                    case DummyValue1Changed d:
                        Value1 = d.Value;
                        break;

                    case DummyValue2Changed d:
                        Value2 = d.Value;
                        break;

                    case DummyValue3Changed d:
                        Value3 = d.Value;
                        break;

                    case DummyDisabled:
                        Disabled = true;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
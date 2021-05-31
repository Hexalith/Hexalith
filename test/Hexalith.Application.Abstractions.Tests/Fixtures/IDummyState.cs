namespace Hexalith.Application.Abstractions.Tests.Fixtures
{
    using System.Collections.Generic;

    public interface IDummyState
    {
        bool Disabled { get; set; }
        bool Value1 { get; set; }
        int Value2 { get; set; }
        string Value3 { get; set; }

        void Apply(IEnumerable<object> events);
    }
}
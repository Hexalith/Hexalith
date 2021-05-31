namespace Hexalith.Application.Abstractions.Tests.Fixtures
{
    public sealed class DummyDisabled
    {
        public string DummyId { get; init; } = string.Empty;
    }

    public sealed class DummyValue1Changed
    {
        public string DummyId { get; init; } = string.Empty;
        public bool Value { get; init; }
    }

    public sealed class DummyValue2Changed
    {
        public string DummyId { get; init; } = string.Empty;
        public int Value { get; init; }
    }

    public sealed class DummyValue3Changed
    {
        public string DummyId { get; init; } = string.Empty;
        public string Value { get; init; }
    }

    public sealed class NewDummyAdded
    {
        public string DummyId { get; init; } = string.Empty;
        public bool Value1 { get; init; }
        public int Value2 { get; init; }
        public string Value3 { get; init; }
    }
}
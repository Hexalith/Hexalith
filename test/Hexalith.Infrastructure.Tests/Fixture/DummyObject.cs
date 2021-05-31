namespace Hexalith.Infrastructure.Tests.Fixture
{
    using System.Collections.Generic;

    public class DummyBaseObject
    {
        public int IntBaseProperty { get; }
        public string StringBaseProperty { get; } = string.Empty;
        public IEnumerable<string> StringsBaseProperty { get; } = new List<string>();
    }

    public class DummyObject : DummyBaseObject
    {
        public int IntProperty { get; set; }
        public string StringInitOnlySetProperty { get; init; } = string.Empty;
        public string StringPrivateSetProperty { get; private set; } = string.Empty;
        public string StringProperty { get; set; } = string.Empty;
        public string StringReadOnlyProperty { get; } = string.Empty;
        public IEnumerable<string> StringsProperty { get; } = new List<string>();
    }
}
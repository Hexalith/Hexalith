#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable CS0414

namespace Hexalith.Infrastructure.Abstractions.Tests.Fixtures
{
    using System;

    public class DummyObject
    {
        private string _aPrivateFieldNullString = null;
        private string _aPrivateFieldString = "a private field string value 1";
        public DateTime ADateTime { get; set; } = DateTime.MinValue;
        public DateTimeOffset ADateTimeOffset { get; set; } = DateTimeOffset.MinValue;
        public decimal ADecimal { get; set; }
        public double ADouble { get; set; }
        public int AnInteger { get; set; }
        public string ANullString { get; set; } = null;
        public string AString { get; set; } = string.Empty;
        public string[] AStringArray { get; set; } = Array.Empty<string>();
        private string APrivateString { get; } = "a private string value.";

        public void SetValues1()
        {
            ADateTime = new DateTime(2010, 5, 30, 11, 59, 22, 165);
            ADateTimeOffset = new DateTimeOffset(2011, 6, 28, 15, 45, 33, 899, new TimeSpan(1, 0, 0));
            ADecimal = 58744.23M;
            ADouble = 256.265;
            AnInteger = 125;
            AString = "a string value";
            AStringArray = new[] { "String 1", "String 2", "String 3" };
        }

        public void SetValues2()
        {
            ADateTime = DateTime.Now;
            ADateTimeOffset = DateTimeOffset.Now;
            ADecimal = 25441.88M;
            ADouble = 876323245.53657;
            AnInteger = 3247;
            AString = "a string value 2";
            AStringArray = new[] { "String 2-1", "String 2-2", "String 2-3" };
        }
    }

    public class DummyObjectWithSubObject : DummyObject
    {
        public DummyObject ASubObject { get; } = new DummyObject();
    }
}
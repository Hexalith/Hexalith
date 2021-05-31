namespace Hexalith.Infrastructure.Dataset.Tests
{
    using System;
    using System.Text;

    using FluentAssertions;

    using Xunit;

    public class CsvDataSetTest
    {
        public CsvDataSetTest()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Fact]
        public void Read_simple_1252_csv()
        {
            Read_simple_csv(Encoding.GetEncoding(1252));
        }

        [Fact]
        public void Read_simple_Default_csv()
        {
            Read_simple_csv(Encoding.Default);
        }

        [Fact]
        public void Read_simple_Latin1_csv()
        {
            Read_simple_csv(Encoding.Latin1);
        }

        [Fact]
        public void Read_simple_UTF8_csv()
        {
            Read_simple_csv(Encoding.UTF8);
        }

        private static void Read_simple_csv(Encoding encoding)
        {
            const string value1 = "value 1 ééààçç\\|$£²°^¨*µ";
            encoding.GetString(encoding.GetBytes(value1)).Should().Be(value1);

            var csv64 = Convert.ToBase64String(encoding
                        .GetBytes($"Fld1;Fld2;Fld3\n1;\"{value1}\";12.25\n2;\"value 2\";22.45"));
            var csv = new CsvDataSet(csv64);
            var ds = csv.Dataset;
            ds.Tables.Count.Should().Be(1);
            var table = ds.Tables[0];
            table.Rows.Count.Should().Be(2);
            table.Columns.Count.Should().Be(3);
            table.Columns[0].ColumnName.Should().Be("Fld1");
            table.Columns[1].ColumnName.Should().Be("Fld2");
            table.Columns[2].ColumnName.Should().Be("Fld3");
            table.Columns[0].DataType.Should().Be(typeof(string));
            table.Columns[1].DataType.Should().Be(typeof(string));
            table.Columns[2].DataType.Should().Be(typeof(string));
            table.Rows[0][0].Should().Be("1");
            table.Rows[0][1].Should().Be(value1);
            table.Rows[0][2].Should().Be("12.25");
            table.Rows[1][0].Should().Be("2");
            table.Rows[1][1].Should().Be("value 2");
            table.Rows[1][2].Should().Be("22.45");
        }
    }
}
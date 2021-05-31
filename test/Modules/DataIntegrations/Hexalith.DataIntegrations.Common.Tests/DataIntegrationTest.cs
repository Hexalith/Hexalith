namespace Hexalith.DataIntegrations.Common.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Hexalith.DataIntegrations.Contracts.Events;
    using Hexalith.DataIntegrations.Domain;
    using Hexalith.DataIntegrations.Domain.States;

    using FluentAssertions;

    using Xunit;

    public class DataIntegrationTest
    {
        public DataIntegrationTest()
        {
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        [Fact]
        public async Task Normalize_Csv_should_return_table()
        {
            DataIntegration di = new("id1", new DataIntegrationState
            {
                Name = "test normalize",
                Description = "my normalization test",
                DocumentName = "test.csv",
                DocumentType = "Csv",
                Document = Convert.ToBase64String(
                    Encoding
                        .UTF8
                        .GetBytes("Fld1;Fld2;Fld3\n1;\"value 1\";12.25\n2;\"value 2\";22.45")
                )
            });
            var events = await di.Normalize();
            events.Should().HaveCount(1);
            var e = (DataIntegrationNormalized)events.First();
            e.Name.Should().Be("test normalize");
            e.Description.Should().Be("my normalization test");
            ((IDictionary)e.Data).Should().HaveCount(1);
            ((IDictionary<string, object>)e.Data).Should().ContainKey("Table");
            var table = (IEnumerable<dynamic>)e.Data["Table"];
            table.Should().HaveCount(2);
            dynamic row1 = (IDictionary<string, object>)table.First();
            dynamic row2 = (IDictionary<string, object>)table.Skip(1).First();
            ((string)row1.Fld1).Should().Be("1");
            ((string)row1.Fld2).Should().Be("value 1");
            ((string)row1.Fld3).Should().Be("12.25");
            ((string)row2.Fld1).Should().Be("2");
            ((string)row2.Fld2).Should().Be("value 2");
            ((string)row2.Fld3).Should().Be("22.45");
        }
    }
}
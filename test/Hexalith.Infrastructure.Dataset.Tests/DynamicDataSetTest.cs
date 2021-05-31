namespace Hexalith.Infrastructure.Json.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.Json;

    using Hexalith.Infrastructure.Dataset;

    using FluentAssertions;

    using Xunit;

    public class DynamicDataSetTest
    {
        [Fact]
        public void Empty_dataset_should_have_empty_field_list()
        {
            DataSet dataset = new();
            DynamicDataSet dynData = new(dataset);
            var empty = (IEnumerable)dynData.Value;
            empty.Should().BeEmpty();
        }

        [Fact]
        public void Empty_dataset_should_produce_empty_array()
        {
            DataSet dataset = new();
            DynamicDataSet dynData = new(dataset);
            IEnumerable value = dynData.Value;
            value.Should().BeEmpty();
            string strValue = JsonSerializer.Serialize(dynData.Value);
            strValue.Should().Be(JsonSerializer.Serialize(Array.Empty<string>()));
        }

        [Fact]
        public void One_datatable_should_have_one_level_fields()
        {
            DataSet dataset = new();
            DataTable table = new("my table");
            table.Columns.Add(new DataColumn("Id", typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            table.Rows.Add(1, "tezzz");
            dataset.Tables.Add(table);
            DynamicDataSet dynData = new(dataset);
            var values = (IEnumerable<dynamic>)dynData.Value;
            values.Should().HaveCount(1);
            var value = values.First();
            ((int)value.Id).Should().Be(1);
            ((string)value.Name).Should().Be("tezzz");
        }

        [Fact]
        public void One_datatable_should_produce_simple_array()
        {
            var one = new { Id = 1, Name = "One" };
            var two = new { Id = 2, Name = "Two" };
            DataSet dataset = new();
            DataTable table = new("my table");
            table.Columns.Add(new DataColumn("Id", typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            table.Rows.Add(one.Id, one.Name);
            table.Rows.Add(two.Id, two.Name);
            dataset.Tables.Add(table);
            DynamicDataSet dynData = new(dataset);
            var values = (IEnumerable<dynamic>)dynData.Value;
            values.Should().HaveCount(2);
            var value = values.First();
            ((int)value.Id).Should().Be(1);
            ((string)value.Name).Should().Be("One");
            value = values.Skip(1).First();
            ((int)value.Id).Should().Be(2);
            ((string)value.Name).Should().Be("Two");
        }

        [Fact]
        public void One_empty_datatable_should_produce_empty_array()
        {
            DataSet dataset = new();
            dataset.Tables.Add(new DataTable("my table"));
            DynamicDataSet dynData = new(dataset);
            var empty = (IEnumerable)dynData.Value;
            empty.Should().BeEmpty();
            JsonSerializer.Serialize(empty).Should().Be(JsonSerializer.Serialize(Array.Empty<string>()));
        }

        [Fact]
        public void Two_datatable_should_produce_two_named_arrays()
        {
            var t1f1 = new { Table1Id = 1, Name = "One" };
            var t1f2 = new { Table1Id = 2, Name = "Two" };
            var t2f1 = new { Table2Id = 3, Description = "Three", IsEnabled = true };
            var t2f2 = new { Table2Id = 4, Description = "Four", IsEnabled = false };
            var t2f3 = new { Table2Id = 5, Description = "Five", IsEnabled = true };
            DataSet dataset = new();
            DataTable table1 = new("First");
            table1.Columns.Add(new DataColumn("Table1Id", typeof(int)));
            table1.Columns.Add(new DataColumn("Name", typeof(string)));
            table1.Rows.Add(t1f1.Table1Id, t1f1.Name);
            table1.Rows.Add(t1f2.Table1Id, t1f2.Name);
            dataset.Tables.Add(table1);
            DataTable table2 = new("Second");
            table2.Columns.Add(new DataColumn("Table2Id", typeof(int)));
            table2.Columns.Add(new DataColumn("Description", typeof(string)));
            table2.Columns.Add(new DataColumn("IsEnabled", typeof(bool)));
            table2.Rows.Add(t2f1.Table2Id, t2f1.Description, t2f1.IsEnabled);
            table2.Rows.Add(t2f2.Table2Id, t2f2.Description, t2f2.IsEnabled);
            table2.Rows.Add(t2f3.Table2Id, t2f3.Description, t2f3.IsEnabled);
            dataset.Tables.Add(table2);
            DynamicDataSet dynData = new(dataset);
            var first = (IEnumerable<dynamic>)dynData.Value.First;
            var second = (IEnumerable<dynamic>)dynData.Value.Second;
            first.Should().HaveCount(2);
            second.Should().HaveCount(3);
            var value = first.First();
            ((int)value.Table1Id).Should().Be(1);
            ((string)value.Name).Should().Be("One");
            value = first.Skip(1).First();
            ((int)value.Table1Id).Should().Be(2);
            ((string)value.Name).Should().Be("Two");
            value = second.First();
            ((int)value.Table2Id).Should().Be(3);
            ((string)value.Description).Should().Be("Three");
            ((bool)value.IsEnabled).Should().Be(true);
            value = second.Skip(1).First();
            ((int)value.Table2Id).Should().Be(4);
            ((string)value.Description).Should().Be("Four");
            ((bool)value.IsEnabled).Should().Be(false);
            value = second.Skip(2).First();
            ((int)value.Table2Id).Should().Be(5);
            ((string)value.Description).Should().Be("Five");
            ((bool)value.IsEnabled).Should().Be(true);
        }
    }
}
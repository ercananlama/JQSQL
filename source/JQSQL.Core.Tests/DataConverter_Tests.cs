using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using JQSQL.Core.Data;

namespace JQSQL.Core.Tests
{
    [TestFixture]
    public class DataConverter_Tests
    {
        private const string TestDataFile = "TestData.json";
        private string jsonTestData;

        [TestFixtureSetUp]
        public void FunctionTests_SetUp()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TestDataFile)))
            {
                jsonTestData = reader.ReadToEnd();
            }
        }

        [Test]
        public void When_given_expr_refers_dictionary_return_keys_as_columns()
        {
            JSON json = new JSON();
            var searchResult = json.SearchValue(jsonTestData, "Messages[0]");

            Converter converter = new Converter();
            var data = converter.ConvertToRecord(searchResult);

            Assert.AreEqual(data.Length, 1, "Wrong number of results");
            Assert.AreEqual(data[0].FieldCount, 4, "Wrong field count of result");
            // Validate column names
            Assert.AreEqual(data[0].GetName(0), "Title", "1. field has invalid name");
            Assert.AreEqual(data[0].GetName(1), "To", "2. field has invalid name");
            Assert.AreEqual(data[0].GetName(2), "Sends", "3. field has invalid name");
            Assert.AreEqual(data[0].GetName(3), "Receives", "4. field has invalid name");
            // Validate row values
            Assert.AreEqual(data[0].GetString(0), "Heyy", "Invalid data on 1. field");
            Assert.AreEqual(data[0].GetString(1), "Thelove", "Invalid data on 2. field");
            Assert.IsNotNullOrEmpty(data[0].GetString(2), "Empty data on 3. field");
            Assert.IsNotNullOrEmpty(data[0].GetString(3), "Empty data on 4. field");
        }

        [Test]
        public void When_given_expr_refers_value_array_return_multiple_rows_with_single_column()
        {
            JSON json = new JSON();
            var searchResult = json.SearchValue(jsonTestData, "Messages.Sends.Content");

            Converter converter = new Converter();
            var data = converter.ConvertToRecord(searchResult);

            Assert.AreEqual(data.Length, 3, "Wrong number of results");
            Assert.AreEqual(data[0].FieldCount, 1, "Wrong field count of result");
            // Validate row values
            Assert.AreEqual(data[0].GetString(0), "Whats up?", "Invalid data on 1. row");
            Assert.AreEqual(data[1].GetString(0), "Me too. Nice to hear!", "Invalid data on 2. row");
            Assert.AreEqual(data[2].GetString(0), "How is life going on?", "Invalid data on 3. row");
        }

        [Test]
        public void When_given_expr_refers_object_array_return_multiple_rows_with_multiple_columns_based_on_first_row()
        {
            JSON json = new JSON();
            var searchResult = json.SearchValue(jsonTestData, "Messages.Receives");

            Converter converter = new Converter();
            var data = converter.ConvertToRecord(searchResult);

            Assert.AreEqual(data.Length, 3, "Wrong number of results");
            Assert.AreEqual(data[0].FieldCount, 3, "Wrong field count of result");
            // Validate column names
            Assert.AreEqual(data[0].GetName(0), "Content", "1. field has invalid name");
            Assert.AreEqual(data[0].GetName(1), "SendDate", "2. field has invalid name");
            Assert.AreEqual(data[0].GetName(2), "AttachmentCount", "3. field has invalid name");
            // 1. row data validation
            Assert.AreEqual(data[0].GetString(0), "Great you?", "Invalid data on 1. column of 1. row");
            Assert.AreEqual(data[0].GetDateTime(1), new DateTime(2013, 1, 21), "Invalid data on 2. column of 1. row");
            Assert.AreEqual(data[0].GetDouble(2), 0, "Invalid data on 3. column of on 1. row");
            // 2. row data validation
            Assert.AreEqual(data[1].GetString(0), "Ok bye", "Invalid data on 1. column of 2. row");
            Assert.AreEqual(data[1].GetDateTime(1), new DateTime(2013, 1, 25), "Invalid data on 2. column of 2. row");
            Assert.AreEqual(data[1].GetDouble(2), 1, "Invalid data on 3. column of on 2. row");
            // 3. row data validation
            Assert.AreEqual(data[2].GetString(0), "Fine. Nothing new yet! You?", "Invalid data on 1. column of 3. row");
            Assert.AreEqual(data[2].GetDateTime(1), new DateTime(2013, 3, 12), "Invalid data on 2. column of 3. row");
            Assert.IsTrue(data[2].GetSqlDouble(2).IsNull, "Invalid data on 3. column of on 3. row");
        }

        [Test]
        public void When_given_expr_refers_simple_value_return_single_row_with_single_column()
        {
            JSON json = new JSON();
            var searchResult = json.SearchValue(jsonTestData, "Messages[0].Sends[0].Content");

            Converter converter = new Converter();
            var data = converter.ConvertToRecord(searchResult);

            Assert.AreEqual(data.Length, 1, "Wrong number of results");
            Assert.AreEqual(data[0].FieldCount, 1, "Wrong field count of result");
            Assert.AreEqual(data[0].GetString(0), "Whats up?", "Invalid field value");
        }
    }
}

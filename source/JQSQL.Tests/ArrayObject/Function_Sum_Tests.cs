using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Tests.ArrayObject
{
    [TestFixture]
    public class Function_Sum_Tests : Base_Function_Tests
    {
        [Test]
        [TestCaseSource("ExpectedValues")]
        public void When_root_node_begins_with_arrays_return_sum_of_all_values(string expression, double expected)
        {
            var found = Functions.Sum(JsonTestData, expression);

            Assert.AreEqual(expected, found.Value);
        }

        [Test]
        [TestCaseSource("NotCalculatedElements")]
        public void When_element_not_able_to_aggregated_return_null(string expression)
        {
            var found = Functions.Sum(JsonTestData, expression);

            Assert.IsTrue(found.IsNull);
        }

        public static IEnumerable<TestCaseData> ExpectedValues
        {
            get
            {
                yield return new TestCaseData("Stocks.Sales.ItemCount", 249).SetName("No indexed arrays");
                yield return new TestCaseData("[0].Stocks.Sales.ItemCount", 170).SetName("Only root indexed");
                yield return new TestCaseData("Stocks.Sales[1].ItemCount", 139).SetName("Only child indexed");
                yield return new TestCaseData("[0].Stocks.Purchases[1].ItemCount", 35).SetName("Single element");
            }
        }

        public static IEnumerable<TestCaseData> NotCalculatedElements
        {
            get
            {
                yield return new TestCaseData("Title").SetName("Text value");
                yield return new TestCaseData("Stocks.Sales.Date").SetName("Date value");
                yield return new TestCaseData("Stocks.Sales").SetName("Complex value");
            }
        }
    }
}

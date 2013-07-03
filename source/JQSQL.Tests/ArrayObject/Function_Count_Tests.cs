using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Tests.ArrayObject
{
    [TestFixture]
    public class Function_Count_Tests : Base_Function_Tests
    {
        [Test]
        [TestCaseSource("ExpectedValues")]
        public void When_root_node_begins_with_arrays_return_count_of_all_values(string expression, double expected)
        {
            var found = Functions.Count(JsonTestData, expression);

            Assert.AreEqual(expected, found);
        }

        public static IEnumerable<TestCaseData> ExpectedValues
        {
            get
            {
                yield return new TestCaseData("Stocks.Sales.ItemCount", 4).SetName("No indexed arrays");
                yield return new TestCaseData("[0].Stocks.Sales.ItemCount", 2).SetName("Only root indexed");
                yield return new TestCaseData("Stocks.Sales[1].ItemCount", 2).SetName("Only child indexed");
                yield return new TestCaseData("[0].Stocks.Purchases[1].ItemCount", 1).SetName("Single element");
                yield return new TestCaseData("Title", 2).SetName("Text element");
                yield return new TestCaseData("Stocks.Sales.Date", 4).SetName("Date element");
                yield return new TestCaseData("Stocks.Sales", 4).SetName("Complex element");
            }
        }
    }
}

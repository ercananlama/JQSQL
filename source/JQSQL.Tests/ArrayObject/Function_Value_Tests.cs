using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Tests.ArrayObject
{
    [TestFixture]
    public class Function_Value_Tests : Base_Function_Tests
    {
        [Test]
        [TestCaseSource("RootProperties")]
        public void When_root_property_supplied_return_correct_value(string propertyValue, string propertyName)
        {
            var foundPropertyValue = Functions.Value(JsonTestData, propertyName);

            Assert.AreEqual(propertyValue, foundPropertyValue);
        }

        [Test]
        [TestCaseSource("ChildProperties")]
        public void When_child_property_supplied_return_correct_value(string propertyValue, string propertyName)
        {
            var foundPropertyValue = Functions.Value(JsonTestData, propertyName);

            Assert.AreEqual(propertyValue, foundPropertyValue);
        }

        [Test]
        [TestCaseSource("NotAvailableIndexProperties")]
        public void When_given_index_not_found_return_null(string expression)
        {
            var foundExpressionValue = Functions.Value(JsonTestData, expression);

            Assert.AreEqual(null, foundExpressionValue);
        }

        public static IEnumerable<TestCaseData> RootProperties
        {
            get
            {
                yield return new TestCaseData("12345", "[0].ProductCode").SetName("ProductCode");
                yield return new TestCaseData("MyPhone", "[0].Title").SetName("Title");
                yield return new TestCaseData("Connect and communicate easily, best price", "[1].Description").SetName("Description");
                yield return new TestCaseData("650", "[1].Price").SetName("Price");
                yield return new TestCaseData("$", "[0].Currency").SetName("Currency");
            }
        }

        public static IEnumerable<TestCaseData> ChildProperties
        {
            get
            {
                yield return new TestCaseData("2013-01-03", "[0].Stocks.Sales[1].Date").SetName("SaleDate");
                yield return new TestCaseData("50", "[0].Stocks.Sales[0].ItemCount").SetName("SaleItemCount");
                yield return new TestCaseData("2013-01-22", "[1].Stocks.Purchases[0].Date").SetName("PurchaseDate");
                yield return new TestCaseData("23", "[1].Stocks.Purchases[0].ItemCount").SetName("PurchaseItemCount");
            }
        }

        public static IEnumerable<TestCaseData> NotAvailableIndexProperties
        {
            get
            {
                yield return new TestCaseData("[3].ProductCode").SetName("Simple root index");
                yield return new TestCaseData("[5].Stocks.Sales[0]").SetName("Complex root index");
                yield return new TestCaseData("[0].Stocks.Sales[5]").SetName("Child index");
            }
        }
    }
}

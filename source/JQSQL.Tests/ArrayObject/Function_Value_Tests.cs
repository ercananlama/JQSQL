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

        [TestCaseSource("AttributeProperties")]
        public void When_given_property_supplied_with_attribute_filter_values(string expectedResult, string expression)
        {
            var foundResult = Functions.Value(JsonTestData, expression);

            Assert.AreEqual(expectedResult, foundResult);
        }

        [TestCaseSource("NotValidAttributeProperties")]
        public void When_given_property_is_not_applicable_return_null(string expression)
        {
            var found = Functions.Value(JsonTestData, expression);

            Assert.IsNull(found);
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
                yield return new TestCaseData("[Price > 500][2].ProductCode").SetName("Root index with attribute filter");
            }
        }

        public static IEnumerable<TestCaseData> AttributeProperties
        {
            get
            {
                yield return new TestCaseData("12345", "[Price > 1000].ProductCode").SetName("GreaterThanForInt");
                yield return new TestCaseData("19", "Stocks.Sales[Date >= \"2013-01-13\"].ItemCount").SetName("GreaterThanOrEqualForDate");
                yield return new TestCaseData("10", "Stocks.Purchases[Date < \"2013-01-05\"].ItemCount").SetName("LessThanForDate");
                yield return new TestCaseData("13579", "[Title = \"PhoneWorld\"].ProductCode").SetName("EqualForString");
                yield return new TestCaseData("2013-01-13", "Stocks.Sales[ItemCount <= 20].Date").SetName("LessThanOrEqualForInt");
                yield return new TestCaseData("[120,60]", "Stocks.Sales[ItemCount > 50][0].ItemCount").SetName("IndexAttributeTogetherOnChild");
                yield return new TestCaseData("13579", "[Price > 500][1].ProductCode").SetName("IndexAttributeTogetherOnRoot");
                yield return new TestCaseData("2013-01-13", "[Price > 500].Stocks.Sales[ItemCount <= 20].Date").SetName("MultipleAttributesSeperate");
                yield return new TestCaseData("2013-01-03", "[Price > 750].Stocks.Sales[ItemCount > 50][0].Date").SetName("MultipleAttributesSeperateWithIndex");
                yield return new TestCaseData("12345", "[Title   =          \"MyPhone\"      ].ProductCode").SetName("EqualWithSpaces");
            }
        }

        public static IEnumerable<TestCaseData> NotValidAttributeProperties
        {
            get
            {
                yield return new TestCaseData("[Price > \"abc\"].ProductCode").SetName("NotValidConversion");
                yield return new TestCaseData("[Price1 > 123].ProductCode").SetName("NotAvailableItemInFilter");
                yield return new TestCaseData("[Price > 500].ProductCode1").SetName("NotAvailableItemKey");
                yield return new TestCaseData("[Price & 500].ProductCode").SetName("UnknownTokenInFilter");
            }
        }
    }
}

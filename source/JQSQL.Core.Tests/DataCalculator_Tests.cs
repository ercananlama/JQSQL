using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using JQSQL.Core.Data;

namespace JQSQL.Core.Tests
{
    [TestFixture]
    public class DataCalculator_Tests
    {
        [Test]
        public void When_collection_contains_null_take_only_non_null_values_for_calculation()
        {
            List<object> values = new List<object>()
            {
                8,
                2,
                null,
                5
            };

            Calculator calc = new Calculator();
            var sum = calc.GetSum(values);
            var avg = calc.GetAverage(values);
            var max = calc.GetMax(values);
            var min = calc.GetMin(values);

            Assert.AreEqual(15, sum, "Sum failed");
            Assert.AreEqual(5, avg, "Avg failed");
            Assert.AreEqual(8, max, "Max failed");
            Assert.AreEqual(2, min, "Min failed");
        }

        [Test]
        public void When_collection_contains_date_only_max_min_should_return_value()
        {
            List<object> values = new List<object>()
            {
                new DateTime(2012, 1, 1),
                new DateTime(2011, 2, 1),
                new DateTime(2013, 2, 4),
                new DateTime(2012, 8, 4),
                new DateTime(2013, 6, 5),
            };

            Calculator calc = new Calculator();
            var sum = calc.GetSum(values);
            var avg = calc.GetAverage(values);
            var max = calc.GetMax(values);
            var min = calc.GetMin(values);

            Assert.AreEqual(null, sum, "Sum failed");
            Assert.AreEqual(null, avg, "Avg failed");
            Assert.AreEqual(new DateTime(2013, 6, 5), max, "Max failed");
            Assert.AreEqual(new DateTime(2011, 2, 1), min, "Min failed");
        }

        [Test]
        public void When_collection_contains_different_types_of_values_only_take_calculatable_values_for_sum_avg()
        {
            var values1 = new List<object>()
            {
                new DateTime(2012, 1, 1),
                6,
                new DateTime(2013, 2, 4),
                "Hi, I am back",
                12
            };

            var values2 = new List<object>()
            {
                6,
                new DateTime(2012, 1, 1),
                new DateTime(2013, 2, 4),
                "Hi, I am back",
                12
            };

            Calculator calc = new Calculator();
            var sum = calc.GetSum(values2);
            var avg = calc.GetAverage(values2);

            Assert.AreEqual(18, sum, "Sum failed");
            Assert.AreEqual(9, avg, "Avg failed");
        }

        [Test]
        public void When_collection_contains_different_types_of_values_calculate_based_on_first_item_for_max_min()
        {
            var values1 = new List<object>()
            {
                new DateTime(2012, 1, 1),
                6,
                new DateTime(2013, 2, 4),
                "Hi, I am back",
                12
            };

            var values2 = new List<object>()
            {
                6,
                new DateTime(2012, 1, 1),
                new DateTime(2013, 2, 4),
                "Hi, I am back",
                12
            };

            Calculator calc = new Calculator();
            var max1 = calc.GetMax(values1);
            var min1 = calc.GetMin(values1);
            var max2 = calc.GetMax(values2);
            var min2 = calc.GetMin(values2);

            Assert.AreEqual(new DateTime(2013, 2, 4), max1, "Max failed for date");
            Assert.AreEqual(new DateTime(2012, 1, 1), min1, "Min failed for date");
            Assert.AreEqual(12, max2, "Max failed for numeric");
            Assert.AreEqual(6, min2, "Min failed for numeric");
        }
    }
}

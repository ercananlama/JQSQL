using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Core.Tests
{
    [TestFixture]
    public class JSON_Tests
    {
        [Test]
        public void When_array_contains_only_primitive_types_return_same_array_back()
        {
            var array = new SimpleJson.JsonArray();
            array.Add(1);
            array.Add(3);
            array.Add(5);

            var attrFilter = new Elements.JAttributeFilter();
            attrFilter.Attribute = new Elements.JAttribute();
            attrFilter.Attribute.Name = "Test";
            attrFilter.Filter = new Elements.JFilter();
            attrFilter.Filter.FilterType = Elements.JFilter.FilterTypes.Equal;
            attrFilter.Filter.Value = 1;

            JSON json = new JSON();
            var results = json.ApplyFilter(array, attrFilter, (value1, value2) =>
            {
                return Convert.ToInt32(value1) == Convert.ToInt32(value2);
            });

            Assert.AreEqual(results.Count, array.Count);
            Assert.AreEqual(results[0], array[0]);
            Assert.AreEqual(results[1], array[1]);
            Assert.AreEqual(results[2], array[2]);
        }
    }
}

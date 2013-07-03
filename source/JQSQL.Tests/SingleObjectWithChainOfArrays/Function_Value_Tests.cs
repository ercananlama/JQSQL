using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Tests.SingleObjectWithChainOfArrays
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
        public void When_given_property_not_found_return_last_accessed_content()
        {
            var expectedValue = "{\"Content\":\"Ok bye\",\"SendDate\":\"25-01-2013\",\"AttachmentCount\":1}";
            var foundPropertyValue = Functions.Value(JsonTestData, "Messages[0].Receives[1].TestProperty");

            Assert.AreEqual(expectedValue, foundPropertyValue);
        }

        [Test]
        public void When_given_property_refers_an_object_return_it_as_object()
        {
            var expectedValue = "{\"Content\":\"Ok bye\",\"SendDate\":\"25-01-2013\",\"AttachmentCount\":1}";
            var foundPropertyValue = Functions.Value(JsonTestData, "Messages[0].Receives[1]");

            Assert.AreEqual(expectedValue, foundPropertyValue);
        }

        public static IEnumerable<TestCaseData> RootProperties
        {
            get
            {
                yield return new TestCaseData("Chris", "FirstName").SetName("FirstName");
                yield return new TestCaseData("Brown", "LastName").SetName("LastName");
                yield return new TestCaseData("France", "Country").SetName("Country");
                yield return new TestCaseData("26", "Age").SetName("Age");
                yield return new TestCaseData("Theking", "Nickname").SetName("Nickname");
                yield return new TestCaseData("Single", "MaritalStatus").SetName("MaritalStatus");
            }
        }

        public static IEnumerable<TestCaseData> ChildProperties
        {
            get
            {
                yield return new TestCaseData("Heyy", "Messages[0].Title").SetName("MessageTitle");
                yield return new TestCaseData("Thelove", "Messages[0].To").SetName("MessageReceiver");
                yield return new TestCaseData("Whats up?", "Messages[0].Sends[0].Content").SetName("FirstSentMessageContent");
                yield return new TestCaseData("25-01-2013", "Messages[0].Receives[1].SendDate").SetName("SecondReceivedMessageDate");
            }
        }
    }
}

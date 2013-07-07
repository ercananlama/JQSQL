using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Tests.SingleObjectWithChainOfArrays
{
    [TestFixture]
    public class Function_Average_Tests : Base_Function_Tests
    {
        [Test]
        [TestCaseSource("ChainOfArrayItemsAganistExpectedValues")]
        public void When_element_in_chain_of_arrays_requested_return_average_of_all_values(string expression, double expected)
        {
            var found = Functions.Average(JsonTestData, expression);

            Assert.AreEqual(expected, found.Value);
        }

        [Test]
        [TestCaseSource("NotCalculatedElements")]
        public void When_element_not_able_to_aggregated_return_null(string expression)
        {
            var found = Functions.Average(JsonTestData, expression);

            Assert.IsTrue(found.IsNull);
        }

        public static IEnumerable<TestCaseData> ChainOfArrayItemsAganistExpectedValues
        {
            get
            {
                yield return new TestCaseData("Messages.Sends.AttachmentCount", 4.0 / 3.0).
                    SetName("No indexed arrays").
                    SetDescription("Check the average of attachment counts in all sends in all messages");
                yield return new TestCaseData("Messages[0].Sends.AttachmentCount", 1.5).
                    SetName("Only first array indexed").
                    SetDescription("Check the average of attachment counts in all sends in first message");
                yield return new TestCaseData("Messages.Sends[0].AttachmentCount", 1).
                    SetName("Second array indexed along with first array with multiple elements").
                    SetDescription("Check the average of attachment counts in first elements of all sends in all messages");
                yield return new TestCaseData("Messages[0].Sends[0].AttachmentCount", 1).
                    SetName("Single element").
                    SetDescription("Check the average of attachment counts in first send in first message, which should be only 1 item");
            }
        }

        public static IEnumerable<TestCaseData> NotCalculatedElements
        {
            get
            {
                yield return new TestCaseData("Messages.Sends.SendDate").SetName("Date value");
                yield return new TestCaseData("Messages.Receives.Content").SetName("Text value");
                yield return new TestCaseData("Messages.Receives").SetName("Complex value");
            }
        }
    }
}

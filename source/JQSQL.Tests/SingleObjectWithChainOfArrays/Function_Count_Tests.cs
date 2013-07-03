using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace JQSQL.Tests.SingleObjectWithChainOfArrays
{
    [TestFixture]
    public class Function_Count_Tests : Base_Function_Tests
    {
        [Test]
        [TestCaseSource("ChainOfArrayItemsAganistExpectedValues")]
        public void When_element_in_chain_of_arrays_requested_return_count_of_all_values(string expression, double expected)
        {
            var found = Functions.Count(JsonTestData, expression);

            Assert.AreEqual(expected, found);
        }

        public static IEnumerable<TestCaseData> ChainOfArrayItemsAganistExpectedValues
        {
            get
            {
                yield return new TestCaseData("Messages.Sends.AttachmentCount", 3).SetName("No indexed arrays");
                yield return new TestCaseData("Messages[0].Sends.AttachmentCount", 2).SetName("Only root array indexed");
                yield return new TestCaseData("Messages.Sends[0].AttachmentCount", 2).SetName("Only child array indexed");
                yield return new TestCaseData("Messages[0].Sends[0].AttachmentCount", 1).SetName("Single element");
                yield return new TestCaseData("Messages.Sends.SendDate", 3).SetName("Date element");
                yield return new TestCaseData("Messages.Receives.Content", 3).SetName("Text element");
                yield return new TestCaseData("Messages.Receives", 3).SetName("Object element");
                yield return new TestCaseData("Messages.Receives.AttachmentCount", 2).SetName("Missing element");
            }
        }
    }
}

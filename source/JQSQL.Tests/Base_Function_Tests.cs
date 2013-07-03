using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace JQSQL.Tests
{
    public class Base_Function_Tests
    {
        protected virtual string JsonTestDataFilePath { get; private set; }
        protected string JsonTestData { get; set; }

        [TestFixtureSetUp]
        public void FunctionTests_SetUp()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JsonTestDataFilePath)))
            {
                JsonTestData = reader.ReadToEnd();
            }
        }
    }
}

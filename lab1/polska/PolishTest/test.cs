using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Polska;

namespace PolishTest
{
    [TestClass]
    public class test
    {
        Dictionary<String, String> testStrings = new Dictionary<String, String>();
        List<int> calculatedResults = new List<int>();

        public test()
        {
            testStrings.Add("10+(15-5)*2", "10 15 5 - 2 * +");
            testStrings.Add("74+(5-2)*4","74 5 2 - 4 * +");

            calculatedResults.Add(30);
            calculatedResults.Add(86);
        }

        [TestMethod]
        public void TestToPolishMethod()
        {
            foreach(var testPair in testStrings)
            {
                var p = new PolishString(testPair.Key);
                Assert.AreEqual(p.getPolska(), testPair.Value);
            }
        }

        [TestMethod]
        public void TestCalculateMethod()
        {
            var p = new PolishString("10+(15-5)*2");
            Assert.AreEqual(p.calculatePolska(), 30);

            p = new PolishString("74+(5-2)*4");
            Assert.AreEqual(p.calculatePolska(), 86);
        }
    }
}

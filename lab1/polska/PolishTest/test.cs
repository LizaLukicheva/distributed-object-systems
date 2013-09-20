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
            testStrings.Add("74+(5-2)*4", "74 5 2 - 4 * +");
            testStrings.Add("3+4", "3 4 +");
            testStrings.Add("7-2*3", "7 2 3 * -");
            testStrings.Add("2*3-7", "2 3 * 7 -");

            calculatedResults.Add(30);
            calculatedResults.Add(86);
            calculatedResults.Add(7);
            calculatedResults.Add(1);
            calculatedResults.Add(-1);
        }

        [TestMethod]
        public void TestToPolishMethod()
        {
            foreach(var testPair in testStrings)
            {
                var p = new PolishString(testPair.Key);
                Assert.AreEqual(testPair.Value, p.getPolska());
            }
        }

        [TestMethod]
        public void TestCalculateMethod()
        {
            var p = new PolishString("10+(15-5)*2");
            Assert.AreEqual(30, p.calculatePolska());

            p = new PolishString("74+(5-2)*4");
            Assert.AreEqual(86, p.calculatePolska());

            p = new PolishString("3+4");
            Assert.AreEqual(7, p.calculatePolska());

            p = new PolishString("7-2*3");
            Assert.AreEqual(1, p.calculatePolska());

            p = new PolishString("2*3-7");
            Assert.AreEqual(-1, p.calculatePolska());
        }
    }
}

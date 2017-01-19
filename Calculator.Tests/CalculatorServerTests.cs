using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CalculatorCommon.Operations;
using System.IO;

namespace Calculator.Tests
{
    [TestClass]
    public class CalculatorServerTests
    {
        [TestMethod]
        public void DivisionOperation()
        {
            Division d = new Division() { Dividend = 2226, Divisor = 53 };
            DivisionResult dr = d.Calculate() as DivisionResult;
            Assert.AreEqual(dr.Quotient, 42);
            Assert.AreEqual(dr.Rest, 0);
        }

        [TestMethod]
        public void MultiplicationOperation()
        {
            Multiplication m = new Multiplication() { Factors = { 1, 2, 3, 7 } };
            MultiplicationResult mr = m.Calculate() as MultiplicationResult;
            Assert.AreEqual(mr.Product, 42);
        }

        [TestMethod]
        public void SqrtOperation()
        {
            Sqrt s = new Sqrt() { Number = 1764 };
            SqrtResult sr = s.Calculate() as SqrtResult;
            Assert.AreEqual(sr.Square, 42);
        }

        [TestMethod]
        public void SubtractionOperation()
        {
            Subtraction s = new Subtraction() { Minuend = 4312, Subtrahend = 4270 };
            SubtractionResult sr = s.Calculate() as SubtractionResult;
            Assert.AreEqual(sr.Difference, 42);
        }

        [TestMethod]
        public void SummationOperation()
        {
            Summation s = new Summation() { Addends = { 1, 2, 3, 7, 9, 4, 10, 6} };
            SummationResult sr = s.Calculate() as SummationResult;
            Assert.AreEqual(sr.Sum, 42);
        }

        [TestMethod]
        public void DatabaseTest()
        {
            int prevCount = 0;
            try
            {
                prevCount = Database.GetJournalEntries("1").Operations.Count;
            }
            catch (FileNotFoundException) { }
            Summation s = new Summation() { Addends = { 1, 2, 3, 7, 9, 4, 10, 6 } };
            Database.SaveOperationIfNeeded(s, 1);
            Assert.AreEqual(prevCount + 1, Database.GetJournalEntries("1").Operations.Count);
        }
    }
}

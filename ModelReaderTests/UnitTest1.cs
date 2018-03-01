using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelReader.Extensions;

namespace ModelReader.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly static IReadOnlyList<TestClass> list = Enumerable
            .Range(0, 100)
            .Select(_ => new TestClass())
            .ToList();

        [TestMethod]
        public void OrderBy()
        {
            var readFrom = list
                .OrderBy("Name".GetFrom)
                .ToList();

            var selector = list
                .OrderBy(n => n.Name)
                .ToList();

            Assert.IsTrue(selector.SequenceEqual(readFrom));
        }
    }
}

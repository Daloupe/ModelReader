using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelReader.Extensions;
using ModelReader.Sorters;
using MongoDB.Driver;

namespace ModelReader.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly static IReadOnlyList<TestClass> list = Enumerable
            .Range(0, 100)
            .Select(_ => new TestClass())
            .ToList();

        private readonly static IOrderVisitor[] Sorters = new[]
{
            ("Name", "DESC"),
            ("Number", "ASC")
        }
        .Select(OrderFactory.Create)
        .ToArray();

        private readonly static int _skip = 5000;
        private readonly static int _limit = 5000;




        [TestMethod]
        public void OrderBy()
        {
            var readFrom = list
                .OrderBy("Name".GetFrom<TestClass>())
                .ToArray();

            var selector = list
                .OrderBy(n => n.Name)
                .ToArray();

            Assert.IsTrue(selector.SequenceEqual(readFrom));
        }

        [TestMethod]
        public void Sort()
        {
            var readFrom = list
                .OrderBy(Sorters)
                .ToArray();

            var selector = list
                .OrderByDescending(n => n.Name)
                .ThenBy(n => n.Number)
                .ToArray();

            Assert.IsTrue(selector.SequenceEqual(readFrom));
        }

        [TestMethod]
        public void Sort_should_fail()
        {
            var readFrom = list
                .OrderBy(Sorters)
                .ToArray();

            var selector = list
                .OrderByDescending(n => n.Number)
                .ThenBy(n => n.Name)
                .ToArray();

            Assert.IsFalse(selector.SequenceEqual(readFrom));
        }


        private readonly static IAggregateFluent<TestClass> agg = new MongoDB.Driver.MongoClient("mongodb://localhost:27017")
    .GetDatabase("test")
    .GetCollection<TestClass>("testClass")
    .Aggregate();

        [TestMethod]
        public void Matching_before_sort_Is_same_as_Skipping_after_sort()
        {
            var output =  agg
              .Match(TestClass.Filter.Gte("Number", 1074257077))
              .OrderBy("Number".Ascending())
              .Limit(_limit)
              .ToList();

            var output2 = agg
              .OrderBy("Number".Ascending())
              .Skip(_skip)
              .Limit(_limit)
              .ToList();

            Assert.IsTrue(output.SequenceEqual(output2));
        }
    }
}

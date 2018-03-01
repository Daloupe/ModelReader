using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using ModelReader.Extensions;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using ModelReader.Sorters;

namespace ModelReader.Benchmarks
{
    class Program
    {
        //private readonly static IReadOnlyList<TestClass> list = Enumerable
        //.Range(0, 10000)
        //.Select(_ => new TestClass())
        //.ToList();

        static void Main(string[] args)
        {
            //new MongoDB.Driver.MongoClient("mongodb://localhost:27017")
            //.GetDatabase("test")
            //.GetCollection<TestClass>("testClass")
            //.InsertMany(list);
                             
            BenchmarkRunner.Run<InMemoryTester>();
            BenchmarkRunner.Run<MongoTester>();
        }
    }

    [BenchmarkDotNet.Attributes.Jobs.SimpleJob(2, 3, 5)]
    [MemoryDiagnoser]
    public class InMemoryTester
    {
        private readonly static int _limit = 50;

        private readonly static IReadOnlyList<TestClass> list = Enumerable
        .Range(0, 10000)
        .Select(_ => new TestClass())
        .ToList();

        private readonly static IOrderVisitor[] Sorters = new[]
        {
            ("Name", "DESC"),
            ("Number", "ASC")
        }
        .Select(OrderFactory.Create)
        .ToArray();

        [Benchmark]
        public static IReadOnlyList<TestClass> NumberDESC_Control()
        {

            return list
                .OrderByDescending(n => n.Number)
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> NumberDESC()
        {
            return list
                .OrderBy("Number"
                    .Descending())
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> NameDESC_NumberASC_Control()
        {

            return list
                .OrderByDescending(n => n.Name)
                .ThenBy(n => n.Number)
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> NameDESC_NumberASC()
        {
            return list
                .OrderBy(Sorters)
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> BoolASC_Control()
        {
            return list
                .OrderBy(n => n.Bool)
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> BoolASC()
        {
            return list
                .OrderBy("Bool"
                .Ascending())
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> StringASC_Control()
        {
            return list
                .OrderBy(n => n.Name)
                .Take(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> StringASC()
        {
            return list
                .OrderBy("Name"
                .Ascending())
                .Take(_limit)
                .ToList();
        }
    }

    [BenchmarkDotNet.Attributes.Jobs.SimpleJob(2, 3, 5)]
    [MemoryDiagnoser]
    public class MongoTester
    {
        private readonly static int _skip = 0;
        private readonly static int _limit = 50;

        private readonly static IAggregateFluent<TestClass> list = new MongoDB.Driver.MongoClient("mongodb://localhost:27017")
            .GetDatabase("test")
            .GetCollection<TestClass>("testClass")
            .Aggregate();

        [Benchmark]
        public static IReadOnlyList<TestClass> IntASC_FindStart_Control()
        {
            return list
                .Match(TestClass.Filter.Lte("Number", 1074257077))
                .SortBy(n => n.Number)
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> IntASC_FindStart()
        {
            return list
                .OrderBy("Number"
                .Ascending()
                .StartingAt(1074257077))
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> StringASC_Skip_Control()
        {
            return list
                .SortBy(n => n.Name)
                .Skip(_skip)
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> StringASC_Skip()
        {
            return list
                .OrderBy("Name"
                    .Ascending())
                .Skip(_skip)
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> BoolDESC_Control()
        {
            return list
                .SortByDescending(n => n.Bool)
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> BoolDESC()
        {
            return list
                .OrderBy("Bool"
                    .Descending())
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> StringDESC_Control()
        {
            return list
                .Match(TestClass.Filter.Gte("Name", "54466906"))
                .SortByDescending(n => n.Name)
                .Limit(_limit)
                .ToList();
        }

        [Benchmark]
        public static IReadOnlyList<TestClass> StringDESC()
        {
            return list
                .OrderBy("Name"
                    .Descending()
                    .StartingAt("54466906"))
                .Limit(_limit)
                .ToList();
        }
    }

}

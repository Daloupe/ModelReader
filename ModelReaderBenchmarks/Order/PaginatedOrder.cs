using System.Collections.Generic;
using MongoDB.Driver;

namespace ModelReader.Sorters
{
    public struct PaginatedOrder
    {
        PaginatedOrder(List<IOrderVisitor> visitors, object start) : this()
        {
            OrderVisitors = visitors;
            Start = start;
        }

        public IReadOnlyList<IOrderVisitor> OrderVisitors { get; }

        public object Start { get; }

        public static PaginatedOrder Create<T>(List<IOrderVisitor> visitors, T start)
        {
            return new PaginatedOrder(visitors, start);
        }

        public static PaginatedOrder Create<T>(IOrderVisitor visitor, T start)
        {
            return new PaginatedOrder(new List<IOrderVisitor> { visitor }, start);
        }

        public IAggregateFluent<T> Match<T>(IAggregateFluent<T> source)
        {
            var firstVisitor = OrderVisitors[0];

            var filter = (firstVisitor is AscendingVisitor)
                ? Builders<T>.Filter.Lte(firstVisitor.Property, Start)
                : Builders<T>.Filter.Gte(firstVisitor.Property, Start);

            return source.Match(filter);
        }
    }
}

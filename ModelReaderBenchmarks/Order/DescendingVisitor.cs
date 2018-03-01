using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using ModelReader.Extensions;

namespace ModelReader.Sorters
{
    public struct DescendingVisitor : IOrderVisitor
    {
        DescendingVisitor(string prop)
        {
            Property = prop;
        }

        public static DescendingVisitor Create(string property)
        {
            return new DescendingVisitor(property);
        }

        public string Property { get; }

        public IOrderedEnumerable<T>
            Order<T>(IEnumerable<T> source)
        {
            return source.OrderByDescending(Property.GetFrom<T>());
        }

        public IOrderedEnumerable<T>
            Order<T>(IOrderedEnumerable<T> source)
        {
            return source.ThenByDescending(Property.GetFrom<T>());
        }

        public IOrderedQueryable<T>
            Order<T>(IQueryable<T> source)
        {
            return source.OrderByDescending(Property.QueryFrom<T>());
        }

        public IOrderedQueryable<T>
            Order<T>(IOrderedQueryable<T> source)
        {
            return source.ThenByDescending(Property.QueryFrom<T>());
        }

        public IOrderedFindFluent<T, T>
            Order<T>(IFindFluent<T, T> source)
        {
            return source.SortByDescending(Property.QueryFrom<T>());
        }

        public IOrderedFindFluent<T, T>
            Order<T>(IOrderedFindFluent<T, T> source)
        {
            return source.ThenByDescending(Property.QueryFrom<T>());
        }

        public IOrderedAggregateFluent<T>
            Order<T>(IAggregateFluent<T> source)
        {
            return source.SortByDescending(Property.QueryFrom<T>());
        }

        public IOrderedAggregateFluent<T>
            Order<T>(IOrderedAggregateFluent<T> source)
        {
            return source.ThenByDescending(Property.QueryFrom<T>());
        }
    }
}

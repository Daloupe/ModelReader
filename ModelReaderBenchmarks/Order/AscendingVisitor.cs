using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using ModelReader.Extensions;

namespace ModelReader.Sorters
{
    public struct AscendingVisitor : IOrderVisitor
    {
        AscendingVisitor(string prop)
        {
            Property = prop;
        }
        public static AscendingVisitor Create(string property)
        {
            return new AscendingVisitor(property);
        }

        public string Property { get; }

        public IOrderedEnumerable<T>
            Order<T>(IEnumerable<T> source)
        {
            return source.OrderBy(Property.GetFrom<T>());
        }

        public IOrderedEnumerable<T>
            Order<T>(IOrderedEnumerable<T> source)
        {
            return source.ThenBy(Property.GetFrom<T>());
        }

        public IOrderedQueryable<T> 
            Order<T>(IQueryable<T> source)
        {
            return source.OrderBy(Property.QueryFrom<T>());
        }

        public IOrderedQueryable<T> 
            Order<T>(IOrderedQueryable<T> source)
        {
            return source.ThenBy(Property.QueryFrom<T>());
        }

        public IOrderedFindFluent<T, T> 
            Order<T>(IFindFluent<T, T> source)
        {
            return source.SortBy(Property.QueryFrom<T>());
        }

        public IOrderedFindFluent<T, T> 
            Order<T>(IOrderedFindFluent<T, T> source)
        {
            return source.ThenBy(Property.QueryFrom<T>());
        }

        public IOrderedAggregateFluent<T> 
            Order<T>(IAggregateFluent<T> source)
        {
            return source.SortBy(Property.QueryFrom<T>());
        }

        public IOrderedAggregateFluent<T> 
            Order<T>(IOrderedAggregateFluent<T> source)
        {
            return source.ThenBy(Property.QueryFrom<T>());
        }
    }

}

using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;

namespace ModelReader.Sorters
{
    public interface IOrderVisitor
    {
        string Property { get; }

        IOrderedEnumerable<T> Order<T>(IEnumerable<T> source);
        IOrderedEnumerable<T> Order<T>(IOrderedEnumerable<T> source);
        IOrderedQueryable<T> Order<T>(IQueryable<T> source);
        IOrderedQueryable<T> Order<T>(IOrderedQueryable<T> source);
        IOrderedFindFluent<T, T> Order<T>(IFindFluent<T, T> source);
        IOrderedFindFluent<T, T> Order<T>(IOrderedFindFluent<T, T> source);
        IOrderedAggregateFluent<T> Order<T>(IAggregateFluent<T> source);
        IOrderedAggregateFluent<T> Order<T>(IOrderedAggregateFluent<T> source);
    }
}

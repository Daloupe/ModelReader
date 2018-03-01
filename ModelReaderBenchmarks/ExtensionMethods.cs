using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ModelReader.Sorters;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ModelReader.Extensions
{
    public static class ExtensionMethods
    {
        public static IOrderVisitor Sort(this string source, string direction)
        {
            return OrderFactory.Create(source, direction);
        }

        public static IOrderVisitor Ascending(this string source)
        {
            return OrderFactory.Create(source, "ASC");
        }

        public static IOrderVisitor Descending(this string source)
        {
            return OrderFactory.Create(source, "DESC");
        }

        public static PaginatedOrder StartingAt<T>(this IOrderVisitor source, T from)
        {
            return PaginatedOrder.Create<T>(source, from);
        }

        public static PaginatedOrder StartingAt<T>(this List<IOrderVisitor> source, T from)
        {
            return PaginatedOrder.Create<T>(source, from);
        }

        public static IAggregateFluent<T> OrderBy<T>(this IAggregateFluent<T> source, PaginatedOrder pagination)
        {
            return pagination
                .Match(source)
                .OrderBy(pagination.OrderVisitors.ToArray());
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, params IOrderVisitor[] visitors)
        {
            if (visitors.Length == 0)
            {
                return source.OrderBy(n => n);
            }

            if (visitors.Length == 1)
            {
                return visitors[0].Order(source);
            }

            return visitors.Skip(1).Aggregate
            (
                visitors[0].Order(source),
                (collection, visitor) => visitor.Order(collection)
            );
        }

        public static IOrderedEnumerable<T> OrderBy<T>(this IEnumerable<T> source, params IOrderVisitor[] visitors)
        {
            if (visitors.Length == 0)
            {
                return source.OrderBy(n => n);
            }

            if (visitors.Length == 1)
            {
                return visitors[0].Order(source);
            }

            return visitors.Skip(1).Aggregate
            (
                visitors[0].Order(source),
                (collection, visitor) => visitor.Order(collection)
            );
        }

        public static IOrderedFindFluent<T, T> OrderBy<T>(this IFindFluent<T, T> source, params IOrderVisitor[] visitors)
        {
            if (visitors.Length == 0)
            {
                return source.SortBy(n => n);
            }

            if (visitors.Length == 1)
            {
                return visitors[0].Order(source);
            }

            return visitors.Skip(1).Aggregate
            (
                visitors[0].Order(source),
                (collection, visitor) => visitor.Order(collection)
            );
        }

        public static IOrderedAggregateFluent<T> OrderBy<T>(this IAggregateFluent<T> source, params IOrderVisitor[] visitors)
        {
            if (visitors.Length == 0)
            {
                return source.SortBy(n => n);
            }

            if (visitors.Length == 1)
            {
                return visitors[0].Order(source);
            }

            return visitors.Skip(1).Aggregate
            (
                visitors[0].Order(source),
                (collection, visitor) => visitor.Order(collection)
            );
        }
    }
}

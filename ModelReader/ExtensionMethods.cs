using System;
using System.Linq.Expressions;

namespace ModelReader.Extensions
{
    public static class ExtensionMethods
    {
        public static Expression<Func<T, object>> QueryFrom<T>(this string source)
        {
            return Readable<T>.Expressers[source];
        }

        public static Func<T, object> GetFrom<T>(this string source)
        {
            return Readable<T>.Readers[source];
        }

        public static Func<T, bool> And<T>(this Predicate<T> first, Predicate<T> second)
        {
            return (value) => first(value) && second(value);
        }
    }
}

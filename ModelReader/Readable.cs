using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ModelReader.Factories;

namespace ModelReader
{
    public delegate object Reader<in T>(T source);

    public static class Readable<T>
    {
        public static IReadOnlyDictionary<string, Func<T, object>> Readers { get; } = ReaderFactory<T>.Create();
        public static IReadOnlyDictionary<string, Expression<Func<T, object>>> Expressers{ get; } = ReaderFactory<T>.CreateExpressions();
    }
}

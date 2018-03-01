using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ModelReader.Extensions;

namespace ModelReader.Factories
{
    public static partial class ReaderFactory<T>
    {
        public static IReadOnlyDictionary<string, Func<T, object>> Create()
        {
            return CreateBase().ToDictionary
            (
                n => n.Name,
                n => DelegateFactory<T>.Create(n.Name, n.PropertyType.IsValueType)
            );
        }

         public static IReadOnlyDictionary<string, Expression<Func<T, object>>> CreateExpressions()
        {
            return CreateBase().ToDictionary
            (
                n => n.Name,
                n => ExpressionFactory<T>.Create(n)
            );
        }
    }

    public static partial class ReaderFactory<T>
    {
        private static readonly Predicate<PropertyInfo> IgnoreAttributes;
        private static readonly Predicate<PropertyInfo> IgnoreArguments;

        static ReaderFactory()
        {
            var AttributesToIgnore = new HashSet<Type>
            { 
            };

            IgnoreAttributes =
                (prop) =>
                !prop.CustomAttributes
                .Any(attr => AttributesToIgnore.Contains(attr.AttributeType));

            var AttributeArgumentsToIgnore = new HashSet<string>
            {
            };

            IgnoreArguments =
                (prop) =>
                !prop.CustomAttributes
                .SelectMany(attr => attr.ConstructorArguments)
                .Any(arg => arg.Value != null && AttributeArgumentsToIgnore.Contains(arg.Value));
        }

        private static IEnumerable<PropertyInfo> CreateBase()
        {
            return typeof(T)
                .GetTypeInfo()
                .GetProperties(BindingFlags.Public | ~BindingFlags.Static)
                .Where(IgnoreAttributes.And(IgnoreArguments));
        }
    }
}

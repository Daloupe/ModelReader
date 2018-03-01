using System;
using System.Linq.Expressions;

namespace ModelReader.Factories
{
    public static class DelegateFactory<T>
    {
        public static Func<T, object> Create(string propertyName, bool isValueType)
        {
            return (isValueType)
                ? MakeValueDelegate(propertyName)
                : MakeDelegate(propertyName);
        }

        private static Func<T, object> MakeDelegate(string propertyName)
        {
            return (Func<T, object>)Delegate.CreateDelegate
            (
                typeof(Func<T, object>),
                typeof(T).GetProperty(propertyName).GetMethod
            );
        }

        private static Func<T, object> MakeValueDelegate(string propertyName)
        {
            var param = Expression.Parameter(typeof(T));
            var expr = Expression.Convert(Expression.Property(param, propertyName), typeof(object));

            return Expression.Lambda<Func<T, object>>(expr, param).Compile();
        }
    }
}

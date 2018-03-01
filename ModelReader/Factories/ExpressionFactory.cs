using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ModelReader.Factories
{
    public static class ExpressionFactory<T>
    {
        public static Expression<Func<T, object>> Create(PropertyInfo property)
        {
            var param = Expression.Parameter(typeof(T));
            var expr = Expression.Convert(Expression.Property(param, property.Name),
                property.PropertyType.IsValueType ? typeof(object) : property.PropertyType);

            return Expression.Lambda<Func<T, object>>(expr, param);
        }
    }
}

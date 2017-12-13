using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;

namespace RendleLabs.Diagnostics.DiagnosticSourceExtensions.ReflectionHelpers
{
    public class SimplePropertyReflector
    {
        private static readonly Type[] FormatAndCultureInfoType = {typeof(string), typeof(CultureInfo)};
        private static readonly Type[] CultureInfoType = {typeof(CultureInfo)};

        private static readonly MethodInfo DateTimeOffsetToString =
            typeof(DateTimeOffset).GetMethod("ToString", new[] {typeof(string), typeof(CultureInfo)});
        private static readonly MethodInfo DateTimeToString =
            typeof(DateTime).GetMethod("ToString", new[] {typeof(string), typeof(CultureInfo)});
        private static readonly MethodInfo TimeSpanToString =
            typeof(TimeSpan).GetMethod("ToString", new[] {typeof(string), typeof(CultureInfo)});
        
        private readonly string _name;
        private readonly Func<object, string> _getter;

        private SimplePropertyReflector(string name, Func<object, string> getter)
        {
            _name = name;
            _getter = getter;
        }

        public string ToString(object o) => _getter(o);

        public static SimplePropertyReflector Generate(PropertyInfo property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));
            var param = Expression.Parameter(typeof(object));
            var castParam = Expression.Convert(param, property.DeclaringType);
            var getter = Expression.Property(castParam, property.Name);
            var toString = ToStringExpression(getter, property.PropertyType);
            
            if (toString == null) return new SimplePropertyReflector(property.Name, _ => null);
            
            return new SimplePropertyReflector(property.Name, Expression.Lambda<Func<object, string>>(toString, param).Compile());
        }

        private static Expression ToStringExpression(MemberExpression expr, Type propertyType)
        {
            if (propertyType == typeof(string))
            {
                return expr;
            }
            
            var invariantCulture =
                Expression.Property(null, typeof(CultureInfo), nameof(CultureInfo.InvariantCulture));
            
            if (propertyType.IsPrimitive)
            {
                var toString = propertyType.GetMethod(nameof(ToString), CultureInfoType);
                if (toString != null)
                {
                    return Expression.Call(expr, toString, invariantCulture);
                }
                toString = propertyType.GetMethod(nameof(ToString), Array.Empty<Type>());
                return toString != null ? Expression.Call(expr, toString, invariantCulture) : null;
            }

            if (propertyType == typeof(DateTimeOffset))
            {
                var format = Expression.Constant("O");
                return Expression.Call(expr, DateTimeOffsetToString, format, invariantCulture);
            }

            if (propertyType == typeof(DateTime))
            {
                var format = Expression.Constant("O");
                return Expression.Call(expr, DateTimeToString, format, invariantCulture);
            }
            
            if (propertyType == typeof(TimeSpan))
            {
                var format = Expression.Constant("c");
                return Expression.Call(expr, TimeSpanToString, format, invariantCulture);
            }
            
            var type = Nullable.GetUnderlyingType(propertyType);
            if (type != null)
            {
                var hasValue = Expression.Property(expr, nameof(Nullable<int>.HasValue));
                var value = Expression.Property(expr, nameof(Nullable<int>.Value));
                return Expression.Condition(hasValue, ToStringExpression(value, type),
                    Expression.Constant(null, typeof(string)));
            }

            return null;
        }
    }
}
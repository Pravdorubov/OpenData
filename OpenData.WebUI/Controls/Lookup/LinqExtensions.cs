using System;
using System.Linq;
using System.Linq.Expressions;

namespace TestApp.Controls.Lookup
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereStartsWith<T>(this IQueryable<T> source, string fieldName, string searchString)
        {
            return Where(source, fieldName, searchString, "StartsWith");
        }

        public static IQueryable<T> WhereContains<T>(this IQueryable<T> source, string fieldName, string searchString)
        {
            return Where(source, fieldName, searchString, "Contains");
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, bool desc , string property)
        {
            return ApplyOrder(source, property, desc ? "OrderByDescending" : "OrderBy");
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

         public static IQueryable<T> Equal<T>(this IQueryable<T> source, string fieldName, string searchString)
         {
             if (searchString == null) searchString = String.Empty;
             var param = Expression.Parameter(typeof(T));
             var prop = Expression.Property(param, fieldName);
             var methodcall = Expression.Equal(prop, Expression.Constant(searchString));
             var lambda = Expression.Lambda<Func<T, bool>>(methodcall, param);
             var request = source.Where(lambda);
             return request;
         }

         public static IQueryable<T> NotEqual<T>(this IQueryable<T> source, string fieldName, string searchString)
         {
             if (searchString == null) searchString = String.Empty;
             var param = Expression.Parameter(typeof(T));
             var prop = Expression.Property(param, fieldName);
             var methodcall = Expression.NotEqual(prop, Expression.Constant(searchString));
             var lambda = Expression.Lambda<Func<T, bool>>(methodcall, param);
             var request = source.Where(lambda);
             return request;
         }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string fieldName, 
            string searchString, string compareFunction)
        {
            if (searchString == null) searchString = String.Empty;
            var param = Expression.Parameter(typeof(T));
            var prop = Expression.Property(param, fieldName);
            var methodcall = Expression.Call(prop,
                                             typeof(String).GetMethod(compareFunction, new[] { typeof(string) }),
// ReSharper disable PossiblyMistakenUseOfParamsMethod
                                             Expression.Constant(value: searchString));
// ReSharper restore PossiblyMistakenUseOfParamsMethod
            var lambda = Expression.Lambda<Func<T, bool>>(methodcall, param);
            var request = source.Where(lambda);
            return request;
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string prop, string methodName)
        {
            var type = typeof(T);
            var param = Expression.Parameter(type);
            var pr = type.GetProperty(prop);
            var expr = Expression.Property(param, type.GetProperty(prop));
            var ptype = pr.PropertyType;
            var delegateType = typeof(Func<,>).MakeGenericType(type, ptype);
            var lambda = Expression.Lambda(delegateType, expr, param);
            var result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(type, ptype)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        } 
    }
}
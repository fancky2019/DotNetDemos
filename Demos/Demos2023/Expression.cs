using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Demos2023
{
    internal class Expression
    {
        private static Expression<Func<T, object>> GetExpression<T>(PropertyInfo propertyInfo)
        {
            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), propertyInfo.Name);
            var propertyAccess = System.Linq.Expressions.Expression.MakeMemberAccess(param, propertyInfo);
            var convert = System.Linq.Expressions.Expression.Convert(propertyAccess, typeof(object));
            Expression<Func<T, object>> propertySelector = System.Linq.Expressions.Expression.Lambda<Func<T, object>>(convert, param);
            return propertySelector;
        }
    }
}

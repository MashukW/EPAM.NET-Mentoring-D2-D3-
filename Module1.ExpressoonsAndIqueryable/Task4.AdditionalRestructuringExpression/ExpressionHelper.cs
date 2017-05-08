using System;
using System.Linq.Expressions;

namespace Task4.AdditionalRestructuringExpression
{
    public static class ExpressionHelper
    {
        // 1. Implement the function's body
        // 2. Explain proposed solution.
        public static Expression<Func<T, bool>> ToBool<T>(this Expression<Func<T, bool?>> expression)
        {
            UnaryExpression body = (UnaryExpression)expression.Body;
            UnaryExpression newUnaryExpression = Expression.MakeUnary(body.NodeType, body.Operand, typeof (bool));
            ParameterExpression parameter = Expression.Parameter(typeof(T), "param");
            
            return Expression.Lambda<Func<T, bool>>(newUnaryExpression, parameter);
        }
    }
}

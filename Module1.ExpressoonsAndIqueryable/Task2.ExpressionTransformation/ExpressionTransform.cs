using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;

namespace Task2.ExpressionTransformation
{
    public class ExpressionTransform : ExpressionVisitor
    {
        private IDictionary<string, object> _constants;

        public ExpressionTransform(IDictionary<string, object> constants)
        {
            if (constants == null)
            {
                throw new ArgumentNullException(nameof(constants));
            }

            _constants = constants;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Add)
            {
                ParameterExpression param = ReplacedBy(node);
            
                if (param != null)
                {
                    return Expression.Increment(param);
                }
            }
            
            if (node.NodeType == ExpressionType.Subtract)
            {
                ParameterExpression param = ReplacedBy(node);
            
                if (param != null)
                {
                    return Expression.Decrement(param);
                }
            }

            return base.VisitBinary(node);
        }

        // Not work!
        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_constants == null)
            {
                return base.VisitParameter(node);
            }

            object valuOfConst;
            _constants.TryGetValue(node.Name, out valuOfConst);

            if (valuOfConst != null)
            {
                if (valuOfConst?.GetType().FullName == node.Type.FullName)
                {
                    // In this case something incorrect.
                    // return Expression.Constant(valuOfConst);
                }
            }

            return base.VisitParameter(node);
        }
        
        private static ParameterExpression ReplacedBy(BinaryExpression node)
        {
            ParameterExpression param = null;
            ConstantExpression constant = null;
            switch (node.Left.NodeType)
            {
                case ExpressionType.Parameter:
                    param = (ParameterExpression)node.Left;
                    break;
                case ExpressionType.Constant:
                    constant = (ConstantExpression)node.Left;
                    break;
            }

            switch (node.Right.NodeType)
            {
                case ExpressionType.Parameter:
                    param = (ParameterExpression)node.Right;
                    break;
                case ExpressionType.Constant:
                    constant = (ConstantExpression)node.Right;
                    break;
            }

            if (param != null && constant != null &&
                constant.Type == typeof (int) && (int) constant.Value == 1)
            {
                return param;
            }

            return null;
        }
    }
}

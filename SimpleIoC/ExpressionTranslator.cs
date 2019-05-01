using System;
using System.Linq.Expressions;

namespace SimpleIoC
{
    internal class ExpressionTranslator
    {
        private static string _propertyName;
        public static string GetPropertyName<TService, TProperty>(Expression<Func<TService, TProperty>> accessExpression)
        {
            _propertyName = string.Empty;

            TranslateExpression(accessExpression);
            
            if(string.IsNullOrEmpty(_propertyName))
                throw new ContainerException($"Can not get property name from expression {accessExpression.Body}");

            return _propertyName;
        }

        private static Expression TranslateExpression(Expression expression)
        {
            if (expression == null)
                return null;

            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    return TranslateUnaryExpression((UnaryExpression)expression);

                case ExpressionType.MemberAccess:
                    return TranslateMemberAccess((MemberExpression)expression);

                case ExpressionType.Lambda:
                    return VisitLambda((LambdaExpression)expression);

                default:
                    throw new ContainerException("Property expression must have MemberAccess type", $"Not supported Expression type {expression.NodeType}.");
            }
        }

        private static Expression TranslateUnaryExpression(UnaryExpression unaryExp)
        {
            if (unaryExp.NodeType == ExpressionType.Convert)
            {
                TranslateExpression(unaryExp.Operand);
            }
            else if (unaryExp.NodeType == ExpressionType.Not)
            {
                TranslateExpression(unaryExp.Operand);
            }
            else
                throw new ContainerException($"Operator {unaryExp.NodeType} not supported.");

            return unaryExp;

        }

        private static Expression VisitLambda(LambdaExpression lambda)
        {

            Expression body = TranslateExpression(lambda.Body);

            return body != lambda.Body ? Expression.Lambda(lambda.Type, body, lambda.Parameters) : lambda;
        }

        private static Expression TranslateMemberAccess(MemberExpression memberExp)
        {
            if (memberExp.Expression == null || memberExp.Expression.NodeType != ExpressionType.Parameter)
                throw new ContainerException($"Member {memberExp.Member.Name} is not supported.");

            _propertyName = memberExp.Member.Name;

            return memberExp;
        }
    }
}

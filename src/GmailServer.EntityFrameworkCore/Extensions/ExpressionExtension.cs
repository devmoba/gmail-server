using System;
using System.Linq.Expressions;

namespace GmailServer.Extensions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<TOuter, TResult>> Apply<TOuter, TInner, TResult>(this Expression<Func<TOuter, TInner>> outer, Expression<Func<TInner, TResult>> inner)
           => Expression.Lambda<Func<TOuter, TResult>>(inner.Body.ReplaceParameter(inner.Parameters[0], outer.Body), outer.Parameters);

        public static Expression<Func<TOuter, TResult>> ApplyTo<TInner, TResult, TOuter>(this Expression<Func<TInner, TResult>> inner, Expression<Func<TOuter, TInner>> outer)
            => outer.Apply(inner);

        public static Expression ReplaceParameter(this Expression expression, ParameterExpression source, Expression target)
            => new ParameterReplacer { source = source, target = target }.Visit(expression);

        class ParameterReplacer : ExpressionVisitor
        {
            public ParameterExpression source;
            public Expression target;
            protected override Expression VisitParameter(ParameterExpression node)
                => node == source ? target : node;
        }
    }
}

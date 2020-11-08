namespace Persistence.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ExpandableQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable
    {
        ExpandableQueryProvider<T> _provider;
        IQueryable<T> _inner;

        internal IQueryable<T> InnerQuery => _inner;

        internal ExpandableQuery(IQueryable<T> inner)
        {
            _inner = inner;
            _provider = new ExpandableQueryProvider<T>(this);
        }

        Expression IQueryable.Expression { get { return _inner.Expression; } }
        Type IQueryable.ElementType { get { return typeof(T); } }
        IQueryProvider IQueryable.Provider { get { return _provider; } }
        public IEnumerator<T> GetEnumerator() { return _inner.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return _inner.GetEnumerator(); }
        public override string ToString() { return _inner.ToString(); }
    }

    class ExpressionExpander : ExpressionVisitor
    {
        Dictionary<ParameterExpression, Expression> _replaceVars = null;

        internal ExpressionExpander() { }

        private ExpressionExpander(Dictionary<ParameterExpression, Expression> replaceVars)
        {
            _replaceVars = replaceVars;
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            if ((_replaceVars != null) && (_replaceVars.ContainsKey(p)))
                return _replaceVars[p];
            else
                return base.VisitParameter(p);
        }

        protected override Expression VisitInvocation(InvocationExpression iv)
        {
            Expression target = iv.Expression;
            if (target is MemberExpression) target = TransformExpr((MemberExpression)target);
            if (target is ConstantExpression) target = ((ConstantExpression)target).Value as Expression;

            LambdaExpression lambda = (LambdaExpression)target;

            Dictionary<ParameterExpression, Expression> replaceVars;
            if (_replaceVars == null)
                replaceVars = new Dictionary<ParameterExpression, Expression>();
            else
                replaceVars = new Dictionary<ParameterExpression, Expression>(_replaceVars);

            try
            {
                for (int i = 0; i < lambda.Parameters.Count; i++)
                    replaceVars.Add(lambda.Parameters[i], iv.Arguments[i]);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Invoke cannot be called recursively - try using a temporary variable.", ex);
            }

            return new ExpressionExpander(replaceVars).Visit(lambda.Body);
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.Name == "Invoke" && m.Method.DeclaringType == typeof(QueryableExtension))
            {
                Expression target = m.Arguments[0];
                if (target is MemberExpression) target = TransformExpr((MemberExpression)target);
                if (target is ConstantExpression) target = ((ConstantExpression)target).Value as Expression;

                LambdaExpression lambda = (LambdaExpression)target;

                Dictionary<ParameterExpression, Expression> replaceVars;
                if (_replaceVars == null)
                    replaceVars = new Dictionary<ParameterExpression, Expression>();
                else
                    replaceVars = new Dictionary<ParameterExpression, Expression>(_replaceVars);

                try
                {
                    for (int i = 0; i < lambda.Parameters.Count; i++)
                        replaceVars.Add(lambda.Parameters[i], m.Arguments[i + 1]);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException("Invoke cannot be called recursively - try using a temporary variable.", ex);
                }

                return new ExpressionExpander(replaceVars).Visit(lambda.Body);
            }

            if (m.Method.Name == "Compile" && m.Object is MemberExpression)
            {
                var me = (MemberExpression)m.Object;
                Expression newExpr = TransformExpr(me);
                if (newExpr != me) return newExpr;
            }

            if (m.Method.Name == "AsExpandable" && m.Method.DeclaringType == typeof(QueryableExtension))
                return m.Arguments[0];

            return base.VisitMethodCall(m);
        }

        Expression TransformExpr(MemberExpression input)
        {
            if (input == null
                || !(input.Member is FieldInfo)
                || !input.Member.ReflectedType.IsNestedPrivate
                || !input.Member.ReflectedType.Name.StartsWith("<>"))
                return input;

            if (input.Expression is ConstantExpression)
            {
                object obj = ((ConstantExpression)input.Expression).Value;
                if (obj == null) return input;
                Type t = obj.GetType();
                if (!t.IsNestedPrivate || !t.Name.StartsWith("<>")) return input;
                FieldInfo fi = (FieldInfo)input.Member;
                object result = fi.GetValue(obj);
                if (result is Expression) return Visit((Expression)result);
            }
            return input;
        }
    }

    class ExpandableQueryProvider<T> : IQueryProvider
    {
        ExpandableQuery<T> _query;

        internal ExpandableQueryProvider(ExpandableQuery<T> query)
        {
            _query = query;
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new ExpandableQuery<TElement>(_query.InnerQuery.Provider.CreateQuery<TElement>(expression.Expand()));
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return _query.InnerQuery.Provider.CreateQuery(expression.Expand());
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute<TResult>(expression.Expand());
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute(expression.Expand());
        }
    }

    public static class QueryableExtension
    {
        public const string ASC = "asc";

        public const string DESC = "desc";

        #region Helper
        private static Expression<Func<T, bool>> TrueExpress<T>() { return f => true; }
        private static Expression<Func<T, bool>> FalseExpress<T>() { return f => false; }

        private static Func<T, bool> TrueFunc<T>() { return f => true; }
        private static Func<T, bool> FalseFunc<T>() { return f => false; }

        private static IOrderedQueryable<T> Ascending<T, U>(IQueryable<T> list, Expression<Func<T, U>> expression)
            => list.OrderBy(expression);

        private static IOrderedQueryable<T> Descending<T, U>(IQueryable<T> list, Expression<Func<T, U>> expression)
            => list.OrderByDescending(expression);

        private static PropertyInfo FindPropertyInfo(Type type, string propertyName)
        {
            PropertyInfo property = null;
            if (propertyName.Contains("."))
            {
                string[] nameParts = propertyName.Split('.');
                property = type.GetProperty(nameParts[0]);

                if (property != null)
                {
                    propertyName = propertyName.Substring(propertyName.IndexOf('.') + 1);
                    property = FindPropertyInfo(property.PropertyType, propertyName);
                }
            }
            else
            {
                property = type.GetProperty(propertyName);
            }
            return property;
        }
        #endregion

        public static Expression Expand(this Expression expr)
        {
            return new ExpressionExpander().Visit(expr);
        }

        public static Expression<TDelegate> Expand<TDelegate>(this Expression<TDelegate> expr)
        {
            return (Expression<TDelegate>)new ExpressionExpander().Visit(expr);
        }

        public static IQueryable<T> AsExpandable<T>(this IQueryable<T> query)
        {
            if (query is ExpandableQuery<T>) return query;
            return new ExpandableQuery<T>(query);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
            {
                expr1 = FalseExpress<T>();
            }

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
            {
                expr1 = TrueExpress<T>();
            }

            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> expr1,
            Func<T, bool> expr2)
        {
            if (expr1 == null)
            {
                expr1 = FalseFunc<T>();
            }

            return finalConditions => new List<Func<T, bool>>() { expr1, expr2 }.Any(x => x(finalConditions));
        }

        public static Func<T, bool> And<T>(this Func<T, bool> expr1,
            Func<T, bool> expr2)
        {
            if (expr1 == null)
            {
                expr1 = TrueFunc<T>();
            }

            return finalConditions => new List<Func<T, bool>>() { expr1, expr2 }.All(x => x(finalConditions));
        }
    }
}

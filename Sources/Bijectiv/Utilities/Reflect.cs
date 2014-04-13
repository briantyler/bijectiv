namespace Bijectiv.Utilities
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    public static class Reflect<T>
    {
        public static ConstructorInfo Constructor([NotNull] Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return ((NewExpression)expression.Body).Constructor;
        }

        public static MethodInfo Method([NotNull] Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return ((MethodCallExpression)expression.Body).Method;
        }

        public static MethodInfo Method<TMember>([NotNull] Expression<Func<T, TMember>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return ((MethodCallExpression)expression.Body).Method;
        }

        public static PropertyInfo Property<TMember>([NotNull] Expression<Func<T, TMember>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return (PropertyInfo)((MemberExpression)expression.Body).Member;
        }
    }
}
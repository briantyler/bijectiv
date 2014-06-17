// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Reflect.cs" company="Bijectiv">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Brian Tyler
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal
//   in the Software without restriction, including without limitation the rights
//   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//   THE SOFTWARE.
// </copyright>
// <summary>
//   Defines the Reflect type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace JetBrains.Annotations
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// A collection of utility methods that perform reflection via expressions.
    /// </summary>
    /// <typeparam name="T">
    /// The type on which to reflect.
    /// </typeparam>
    public static class Reflect<T>
    {
        /// <summary>
        /// Gets a <see cref="ConstructorInfo"/> from an expression.
        /// </summary>
        /// <param name="expression">
        /// An expression of the form: () => new SomeClass(Placeholder.Of&lt;int&gt;()).
        /// </param>
        /// <returns>
        /// The <see cref="ConstructorInfo"/> matching the constructor in the expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression does not describe a constructor.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", 
            Justification = "LambdaExpression base type is not helpful here")]
        [Pure]
        public static ConstructorInfo Constructor([NotNull] Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var @new = expression.Body as NewExpression;
            if (@new == null)
            {
                throw new ArgumentException(
                    "The expression does not describe a constructor. Call as: "
                    + "Reflect<SomeClass>.Constructor(() => new SomeClass(Placeholder.Of<int>())).",
                    "expression");    
            }

            return @new.Constructor;
        }

        /// <summary>
        /// Gets a <see cref="MethodInfo"/> from an action expression.
        /// </summary>
        /// <param name="expression">
        /// An expression of the form: _ => _.SomeMethod(Placeholder.Of&lt;int&gt;()).
        /// </param>
        /// <returns>
        /// The <see cref="MethodInfo"/> matching the method in the expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression does not describe a method.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "LambdaExpression base type is not helpful here")]
        [Pure]
        public static MethodInfo Method([NotNull] Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var call = expression.Body as MethodCallExpression;
            if (call == null)
            {
                throw new ArgumentException(
                    "The expression does not describe a method. Call as: "
                    + "Reflect<SomeClass>.Method(_ => _.SomeMethod(Placeholder.Of<int>())).",
                    "expression");
            }

            return call.Method;
        }

        /// <summary>
        /// Gets a <see cref="MethodInfo"/> from a func expression.
        /// </summary>
        /// <typeparam name="TReturn">
        /// The method's return type (should be inferred from the expression).
        /// </typeparam>
        /// <param name="expression">
        /// An expression of the form: _ => _.SomeMethod(Placeholder.Of&lt;int&gt;()).
        /// </param>
        /// <returns>
        /// The <see cref="MethodInfo"/> matching the method in the expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression does not describe a method.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "LambdaExpression base type is not helpful here")]
        [Pure]
        public static MethodInfo Method<TReturn>([NotNull] Expression<Func<T, TReturn>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var call = expression.Body as MethodCallExpression;
            if (call == null)
            {
                throw new ArgumentException(
                    "The expression does not describe a method. Call as: "
                    + "Reflect<SomeClass>.Method(_ => _.SomeMethod(Placeholder.Of<int>())).",
                    "expression");
            }

            return call.Method;
        }

        /// <summary>
        /// Gets a <see cref="PropertyInfo"/> from a property expression.
        /// </summary>
        /// <typeparam name="TProperty">
        /// The property type (should be inferred from the expression).
        /// </typeparam>
        /// <param name="expression">
        /// An expression of the form: _ => _.SomeProperty.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/> matching the method in the expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression does not describe a property.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "LambdaExpression base type is not helpful here")]
        [Pure]
        public static PropertyInfo Property<TProperty>([NotNull] Expression<Func<T, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(
                    "The expression does not describe member access. Call as: "
                    + "Reflect<SomeClass>.Property(_ => _.SomeProperty).",
                    "expression");
            }

            var property = member.Member as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(
                    "The expression does not describe property access. "
                    + "Perhaps the expression describes access to a field.",
                    "expression");
            }

            return property;
        }

        /// <summary>
        /// Gets a <see cref="FieldInfo"/> from a field expression.
        /// </summary>
        /// <typeparam name="TField">
        /// The field type (should be inferred from the expression).
        /// </typeparam>
        /// <param name="expression">
        /// An expression of the form: _ => _.SomeField.
        /// </param>
        /// <returns>
        /// The <see cref="FieldInfo"/> matching the method in the expression.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the expression does not describe a field.
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
            Justification = "LambdaExpression base type is not helpful here")]
        [Pure]
        public static FieldInfo Field<TField>([NotNull] Expression<Func<T, TField>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(
                    "The expression does not describe member access. Call as: "
                    + "Reflect<SomeClass>.Field(_ => _.SomeField).",
                    "expression");
            }

            var field = member.Member as FieldInfo;
            if (field == null)
            {
                throw new ArgumentException(
                    "The expression does not describe field access. "
                    + "Perhaps the expression describes access to a property.",
                    "expression");
            }

            return field;
        }

        public static MemberInfo FieldOrProperty<TMember>([NotNull] Expression<Func<T, TMember>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var member = expression.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(
                    "The expression does not describe member access. Call as: "
                    + "Reflect<SomeClass>.FieldOrProperty(_ => _.SomeFieldOrProperty).",
                    "expression");
            }

            return member.Member;
        }
    }
}
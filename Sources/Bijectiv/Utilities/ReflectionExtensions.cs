// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensions.cs" company="Bijectiv">
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
//   Defines the ReflectionExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents extensions to reflection types.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the member can be read.
        /// </summary>
        /// <param name="this">
        /// The <see cref="MemberInfo"/> to check for readability.
        /// </param>
        /// <returns>
        /// A value indicating whether the member can be read.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static bool CanRead([NotNull] this MemberInfo @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var field = @this as FieldInfo;
            if (field != null)
            {
                return true;
            }

            var property = @this as PropertyInfo;
            return property != null && property.CanRead;
        }

        /// <summary>
        /// Gets a value indicating whether the member can be written to.
        /// </summary>
        /// <param name="this">
        /// The <see cref="MemberInfo"/> to check for the ability to write.
        /// </param>
        /// <returns>
        /// A value indicating whether the member can be written to.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static bool CanWrite([NotNull] this MemberInfo @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var field = @this as FieldInfo;
            if (field != null && !field.IsInitOnly)
            {
                return true;
            }

            var property = @this as PropertyInfo;
            return property != null && property.CanWrite;
        }

        /// <summary>
        /// Gets an <see cref="Expression"/> that accesses a member represented by a <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="this">
        /// The <see cref="MemberInfo"/> representing the member to access.
        /// </param>
        /// <param name="instance">
        /// The expression that provides the instance that owns the member.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/> representing access to <paramref name="this"/> on <paramref name="instance"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="this"/> is not a <see cref="FieldInfo"/> or a <see cref="PropertyInfo"/>.
        /// </exception>
        public static Expression GetAccessExpression([NotNull] this MemberInfo @this, [NotNull] Expression instance)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var field = @this as FieldInfo;
            if (field != null)
            {
                return Expression.Field(instance, field);
            }

            var property = @this as PropertyInfo;
            if (property != null)
            {
                return Expression.Property(instance, property);
            }

            throw new InvalidOperationException(
                string.Format("Unable to create access expression for member '{0}", @this));
        }

        /// <summary>
        /// Gets the static <see cref="Type"/> of the object referenced by the <paramref name="this"/>.
        /// </summary>
        /// <param name="this">
        /// The <see cref="MemberInfo"/> representing the member.
        /// </param>
        /// <returns>
        /// The static <see cref="Type"/> of the object referenced by the <paramref name="this"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="this"/> is an unrecognised <see cref="MemberInfo"/>.
        /// </exception>
        public static Type GetReturnType([NotNull] this MemberInfo @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            var field = @this as FieldInfo;
            if (field != null)
            {
                return field.FieldType;
            }

            var property = @this as PropertyInfo;
            if (property != null)
            {
                return property.PropertyType;
            }

            var method = @this as MethodInfo;
            if (method != null)
            {
                return method.ReturnType;
            }

            throw new InvalidOperationException(
                string.Format("Unable to determine return type for member '{0}", @this));
        }

        /// <summary>
        /// Gets the default value for <see cref="Type"/> <paramref name="this"/>.
        /// </summary>
        /// <param name="this">
        /// The type for which to get the default value.
        /// </param>
        /// <returns>
        /// The default value <see cref="object"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public static object GetDefault([NotNull] this Type @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            return @this.IsValueType ? Activator.CreateInstance(@this) : null;
        }

        public static TMemberInfo GetBaseDefinition<TMemberInfo>([NotNull] this TMemberInfo @this)
            where TMemberInfo : MemberInfo
        {
            if (@this is FieldInfo)
            {
                return @this;
            }

            var method = @this as MethodInfo;
            if (method != null)
            {
                return method.GetBaseDefinition() as TMemberInfo;
            }

            var property = @this as PropertyInfo;
            if (property == null)
            {
                throw new ArgumentException(
                    string.Format("Cannot get the base definition of member '{0}'", @this), "this");
            }

            var accessor = property.GetAccessors(true)[0];
            if (accessor == null)
            {
                return property as TMemberInfo;
            }

            var baseAccessor = accessor.GetBaseDefinition();
            return baseAccessor == accessor
                ? property as TMemberInfo
                : baseAccessor.DeclaringType.GetProperty(property.Name, ReflectionGateway.All) as TMemberInfo;
        }

        public static bool IsOverride([NotNull] this MemberInfo @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("this");
            }

            if (@this is FieldInfo)
            {
                return false;
            }

            var method = @this as MethodInfo;
            if (method == null)
            {
                var property = @this as PropertyInfo;
                method = property != null ? property.GetAccessors(true)[0] : null;
            }

            return method != null
                && (method.Attributes & MethodAttributes.Virtual) != 0
                && (method.Attributes & MethodAttributes.NewSlot) == 0;
        }
    }
}
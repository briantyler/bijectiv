// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TargetCache.cs" company="Bijectiv">
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
//   Defines the TargetCache type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents a target instance cache.
    /// </summary>
    public class TargetCache : ITargetCache
    {
        /// <summary>
        /// The equality comparer that determines whether a key exists in the cache.
        /// </summary>
        private static readonly IEqualityComparer<Tuple<Type, Type, object>> EqualityComparer = 
            new ObjectReferenceEqualsComparer();

        /// <summary>
        /// The dictionary that performs the cache function.
        /// </summary>
        private readonly IDictionary<Tuple<Type, Type, object>, object> cache = 
            new Dictionary<Tuple<Type, Type, object>, object>(EqualityComparer);

        /// <summary>
        /// Gets the dictionary that performs the cache function.
        /// </summary>
        public IDictionary<Tuple<Type, Type, object>, object> Cache
        {
            get { return this.cache; }
        }

        /// <summary>
        /// Adds a target instance to the cache.
        /// </summary>
        /// <remarks>
        /// The key is composed of <paramref name="sourceType"/>, <paramref name="targetType"/> and
        /// <paramref name="source"/>.
        /// </remarks>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target to add.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the key already exists.
        /// </exception>
        public void Add(
            [NotNull] Type sourceType, 
            [NotNull] Type targetType, 
            [NotNull] object source, 
            [NotNull] object target)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }

            if (targetType == null)
            {
                throw new ArgumentNullException("targetType");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            this.Cache.Add(Tuple.Create(sourceType, targetType, source), target);
        }

        /// <summary>
        /// Tries to get a target from the cache.
        /// </summary>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <param name="targetType">
        /// The target type.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The found target. This value is only valid when the return value is <c>true</c>.
        /// </param>
        /// <returns>
        /// A value indicating whether a target was found.
        /// </returns>
        public bool TryGet(Type sourceType, Type targetType, object source, out object target)
        {
            return this.Cache.TryGetValue(Tuple.Create(sourceType, targetType, source), out target);
        }

        /// <summary>
        /// A <see cref="IEqualityComparer{T}"/> that compares a 3-tuple of <see cref="Type"/>, <see cref="Type"/>
        /// <see cref="object"/> for equality; with two instances being equal if the <see cref="Type"/>s are
        /// equal and the <see cref="object"/>s are reference equal.
        /// </summary>
        private class ObjectReferenceEqualsComparer : IEqualityComparer<Tuple<Type, Type, object>>
        {
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">
            /// The first object to compare.
            /// </param>
            /// <param name="y">
            /// The second object to compare.
            /// </param>
            /// <returns>
            /// A value indicating whether <paramref name="x"/> and <paramref name="y"/> are equal.
            /// </returns>
            [SuppressMessage(
                "Microsoft.Design",
                "CA1062:Validate arguments of public methods",
                MessageId = "0",
                Justification = "Nothing to validate.")]
            [SuppressMessage(
                "Microsoft.Design",
                "CA1062:Validate arguments of public methods",
                MessageId = "1",
                Justification = "Nothing to validate.")]
            public bool Equals(Tuple<Type, Type, object> x, Tuple<Type, Type, object> y)
            {
                return object.Equals(x, y) && ReferenceEquals(x.Item3, y.Item3);
            }

            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">
            /// The object for which a hash code is to be returned.
            /// </param>
            /// <returns>
            /// A hash code for the specified object.
            /// </returns>
            [SuppressMessage(
                "Microsoft.Design", 
                "CA1062:Validate arguments of public methods", 
                MessageId = "0", 
                Justification = "Nothing to validate.")]
            public int GetHashCode(Tuple<Type, Type, object> obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
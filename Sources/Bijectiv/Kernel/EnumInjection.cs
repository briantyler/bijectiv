// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumInjection.cs" company="Bijectiv">
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
//   Defines the EnumInjection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// Represents an <see cref="IInjection"/> between <see cref="Enum"/> and primitive types.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", 
        Justification = "IEnumerable is only implemented to expose the collection initializer.")]
    public class EnumInjection : ITransform, IMerge, IEnumerable
    {
        /// <summary>
        /// The <see cref="TypeCode"/> values of types that are compatible with this <see cref="IInjection"/>.
        /// </summary>
        private static readonly TypeCode[] ValidTypeCodes =
        {
                TypeCode.SByte, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.Byte, 
                TypeCode.UInt16, TypeCode.UInt32, TypeCode.UInt64, TypeCode.String
        };

        /// <summary>
        /// The collection of source to target maps.
        /// </summary>
        private readonly IDictionary<object, object> map = new Dictionary<object, object>();

        /// <summary>
        /// The underlying source type; equal to the source for all types except those amplified by 
        /// <see cref="Nullable{T}" />, where equal to the amplified type.
        /// </summary>
        private readonly Type underlyingSource;

        /// <summary>
        /// The underlying target type; equal to the target for all types except those amplified by 
        /// <see cref="Nullable{T}" />, where equal to the amplified type.
        /// </summary>
        private readonly Type underlyingTarget;

        /// <summary>
        /// A value indicating whether the target type is <see cref="Nullable{T}"/>.
        /// </summary>
        private readonly bool isTargetNullable;

        /// <summary>
        /// A value indicating whether the source type is <see cref="Nullable{T}"/>.
        /// </summary>
        private readonly bool isSourceNullable;

        /// <summary>
        /// Initialises a new instance of the <see cref="EnumInjection"/> class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when neither <paramref name="source"/> nor <paramref name="target"/> is an enum.
        /// Thrown when exactly one of <paramref name="source"/> or <paramref name="target"/> is an enum, but the 
        /// non-enum type is neither an integral type nor a string.
        /// </exception>
        public EnumInjection([NotNull] Type source, [NotNull] Type target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            this.isSourceNullable = true;
            this.underlyingSource = source;
            if (source.IsGenericType && source.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                this.underlyingSource = Nullable.GetUnderlyingType(source);
            }
            else if (source != typeof(string))
            {
                this.isSourceNullable = false;
            }

            this.isTargetNullable = true;
            this.underlyingTarget = target;
            if (target.IsGenericType && target.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                this.underlyingTarget = Nullable.GetUnderlyingType(target);
            }
            else if (target != typeof(string))
            {
                this.isTargetNullable = false;
            }

            if (!this.underlyingSource.IsEnum && !ValidTypeCodes.Contains(Type.GetTypeCode(this.underlyingSource)))
            {
                throw new ArgumentException(
                    string.Format("Source type '{0}' has incompatible type.", source), "source");
            }

            if (!this.underlyingTarget.IsEnum && !ValidTypeCodes.Contains(Type.GetTypeCode(this.underlyingTarget)))
            {
                throw new ArgumentException(
                    string.Format("Target type '{0}' has incompatible type.", target), "target");
            }

            if (!this.underlyingSource.IsEnum && !this.underlyingTarget.IsEnum)
            {
                throw new ArgumentException(
                    string.Format("Neither source {0} nor target {1} type is an Enum.", source, target));
            }

            this.Source = source;
            this.Target = target;
        }

        /// <summary>
        /// Gets the source type supported by the injection.
        /// </summary>
        public Type Source { get; private set; }

        /// <summary>
        /// Gets the target type created by the injection.
        /// </summary>
        public Type Target { get; private set; }

        /// <summary>
        /// Gets the collection of source to target maps.
        /// </summary>
        public IEnumerable<KeyValuePair<object, object>> Map
        {
            get { return this.map; }
        }

        /// <summary>
        /// Adds a new source to target map to the injection.
        /// </summary>
        /// <param name="sourceValue">
        /// The source value.
        /// </param>
        /// <param name="targetValue">
        /// The target value.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="sourceValue"/> is not of the expected type.
        /// Thrown when <paramref name="targetValue"/> is not of the expected type.
        /// </exception>
        public void Add(object sourceValue, object targetValue)
        {
            if ((sourceValue == null && !this.isSourceNullable) || (sourceValue != null && !this.Source.IsInstanceOfType(sourceValue)))
            {
                throw new ArgumentException(
                    string.Format("sourceValue '{0}' is not an instance of type '{1}'.", sourceValue, this.Source),
                    "sourceValue");
            }

            if ((targetValue == null && !this.isTargetNullable) || (targetValue != null && !this.Target.IsInstanceOfType(targetValue)))
            {
                throw new ArgumentException(
                    string.Format("targetValue '{0}' is not an instance of type '{1}'.", targetValue, this.Target),
                    "targetValue");
            }

            this.map[sourceValue ?? Null.Instance] = targetValue;
        }

        /// <summary>
        /// Transforms <paramref name="source"/> into an instance of type <see cref="IInjection.Target"/>;  
        /// using the transformation rules defined for <see cref="IInjection.Source"/> --&gt; 
        /// <see cref="IInjection.Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="context">
        /// The context in which the injection will take place.
        /// </param>
        /// <param name="hint">
        /// A hint that can be used to pass additional information to the injection.
        /// </param>
        /// <returns>
        /// The newly created target instance.
        /// </returns>
        public virtual object Transform(object source, [NotNull] IInjectionContext context, object hint)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            object target;
            if (this.map.TryGetValue(source ?? Null.Instance, out target))
            {
                return target;
            }

            if (source == null)
            {
                if (this.isTargetNullable)
                {
                    return null;
                }

                var message = string.Format(
                    "Attempted to map null onto value type '{0}'. To avoid this exception register null as an "
                    + "explicit injection map",
                    this.Target);

                throw new ArgumentNullException("source", message);
            }

            if (this.underlyingSource.IsEnum && this.underlyingTarget.IsEnum)
            {
                return Enum.Parse(this.underlyingTarget, source.ToString(), false);
            }

            if (this.underlyingSource.IsEnum)
            {
                if (this.underlyingTarget == typeof(string))
                {
                    return source.ToString();
                }

                return Convert.ChangeType(source, this.underlyingTarget, context.Culture);
            }

            if (this.underlyingSource == typeof(string))
            {
                return Enum.Parse(this.underlyingTarget, (string)source, false);
            }

            return Enum.ToObject(this.underlyingTarget, source);
        }

        /// <summary>
        /// Merges <paramref name="source"/> into <paramref name="target"/>; using the transformation rules 
        /// defined by <see cref="IInjection.Source"/> --&lt;  <see cref="IInjection.Target"/>.
        /// </summary>
        /// <param name="source">
        /// The source object.
        /// </param>
        /// <param name="target">
        /// The target object.
        /// </param>
        /// <param name="context">
        /// The context in which the transformation will take place.
        /// </param>
        /// <param name="hint">
        /// A hint that can be used to pass additional information to the injection.
        /// </param>
        /// <returns>
        /// A <see cref="IMergeResult"/> representing the result of the merge.
        /// </returns>
        public IMergeResult Merge(object source, object target, IInjectionContext context, object hint)
        {
            return new MergeResult(PostMergeAction.Replace, this.Transform(source, context, null));
        }

        /// <summary>
        /// Not implemented; exists only to allow collection initializer.
        /// </summary>
        /// <returns>
        /// Nothing; method not implemented.
        /// </returns>
        [ExcludeFromCodeCoverage]
        public IEnumerator GetEnumerator()
        {
            throw new NotSupportedException("Use the Map property to access the registered source --> target maps.");
        }

        /// <summary>
        /// A utility class that allows the null element to be a dictionary key.
        /// </summary>
        private class Null
        {
            /// <summary>
            /// The singleton instance of <see cref="Null"/>.
            /// </summary>
            public static readonly Null Instance = new Null();

            /// <summary>
            /// Prevents a default instance of the <see cref="Null"/> class from being created.
            /// </summary>
            private Null()
            {
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionGateway.cs" company="Bijectiv">
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
//   Defines the ReflectionGateway type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    
    /// <summary>
    /// Represents a gateway to reflection based operations.
    /// </summary>
    public class ReflectionGateway : IReflectionGateway
    {
        /// <summary>
        /// The flags required to bind to public instance members.
        /// </summary>
        public const BindingFlags InstanceBindingFlags =
            BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// The flags required to bind to public or private instance members.
        /// </summary>
        public const BindingFlags NonPublicInstanceBindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        /// <summary>
        /// Gets the collection of <see cref="FieldInfo"/> that belong to <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type from which to get the <see cref="FieldInfo"/>.
        /// </param>
        /// <param name="options">
        /// The reflection options.
        /// </param>
        /// <returns>
        /// The collection of <see cref="FieldInfo"/> that belong to <paramref name="type"/>.
        /// </returns>
        public virtual IEnumerable<FieldInfo> GetFields(Type type, ReflectionOptions options)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var bindingFlags = options.HasFlag(ReflectionOptions.IncludeNonPublic) 
                ? NonPublicInstanceBindingFlags 
                : InstanceBindingFlags;

            return type
                .GetFields(bindingFlags)
                .Where(candidate => !options.HasFlag(ReflectionOptions.CanWrite) || candidate.CanWrite());
        }

        /// <summary>
        /// Gets the collection of <see cref="PropertyInfo"/> that belong to <paramref name="type"/>.
        /// </summary>
        /// <param name="type">
        /// The type from which to get the <see cref="PropertyInfo"/>.
        /// </param>
        /// <param name="options">
        /// The reflection options.
        /// </param>
        /// <returns>
        /// The collection of <see cref="PropertyInfo"/> that belong to <paramref name="type"/>.
        /// </returns>
        public virtual IEnumerable<PropertyInfo> GetProperties(Type type, ReflectionOptions options)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var bindingFlags = options.HasFlag(ReflectionOptions.IncludeNonPublic)
                ? NonPublicInstanceBindingFlags
                : InstanceBindingFlags;

            return type
                .GetProperties(bindingFlags)
                .Where(candidate => !options.HasFlag(ReflectionOptions.CanWrite) || candidate.CanWrite())
                .Where(candidate => !options.HasFlag(ReflectionOptions.CanRead) || candidate.CanRead());
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTestClass.cs" company="Bijectiv">
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
//   Defines the PropertyTestClass type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.TestUtilities.TestTypes
{
    using System.Reflection;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A test class with a property.
    /// </summary>
    public class PropertyTestClass
    {
        /// <summary>
        /// The <see cref="Property"/> <see cref="PropertyInfo"/>.
        /// </summary>
        public static readonly PropertyInfo PropertyPi =
            Reflect<PropertyTestClass>.Property(_ => _.Property);

        /// <summary>
        /// The <see cref="ReadonlyProperty"/> <see cref="PropertyInfo"/>.
        /// </summary>
        public static readonly PropertyInfo ReadonlyPropertyPi =
            Reflect<PropertyTestClass>.Property(_ => _.ReadonlyProperty);

        /// <summary>
        /// The <see cref="WriteonlyProperty"/> <see cref="PropertyInfo"/>.
        /// </summary>
        public static readonly PropertyInfo WriteonlyPropertyPi =
            typeof(PropertyTestClass).GetProperty("WriteonlyProperty");

        /// <summary>
        /// Gets or sets the property.
        /// </summary>
        public int Property { get; set; }

        /// <summary>
        /// Gets the readonly property.
        /// </summary>
        public int ReadonlyProperty
        {
            get { return 1; }
        }

        /// <summary>
        /// Sets the write-only property.
        /// </summary>
        public int WriteonlyProperty 
        { 
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Stub.cs" company="Bijectiv">
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
//   Defines the Stub type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.TestUtilities
{
    using System;
    using System.Reflection;

    using Bijectiv.Configuration;

    using Moq;

    /// <summary>
    /// Stub class helper.
    /// </summary>
    public class Stub
    {
        /// <summary>
        /// Creates an instance of an abstract type, interface.
        /// </summary>
        /// <typeparam name="T">
        /// The abstract type or interface.
        /// </typeparam>
        /// <param name="args">
        /// The constructor parameters.
        /// </param>
        /// <returns>
        /// An instance of the abstract type or interface.
        /// </returns>
        public static T Create<T>(params object[] args)
            where T : class
        {
            try
            {
                return new Mock<T>(args) { CallBase = true }.Object;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Creates a <see cref="InjectionFragment"/>.
        /// </summary>
        /// <param name="inherited">
        /// A value indicating whether the fragment is inherited.
        /// </param>
        /// <param name="category">
        /// The fragment category.
        /// </param>
        /// <typeparam name="TSource">
        /// The fragment source type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// The fragment target type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="InjectionFragment"/>.
        /// </returns>
        public static InjectionFragment Fragment<TSource, TTarget>(
            bool inherited = true, Guid category = default(Guid))
        {
            var fragmentStub = new Mock<InjectionFragment>(MockBehavior.Loose, typeof(TSource), typeof(TTarget));
            fragmentStub.Setup(_ => _.Inherited).Returns(inherited);
            fragmentStub.Setup(_ => _.FragmentCategory).Returns(category);

            try
            {
                return fragmentStub.Object;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Creates a <see cref="InjectionFragment"/>.
        /// </summary>
        /// <param name="category">
        /// The fragment category.
        /// </param>
        /// <typeparam name="TSource">
        /// The fragment source type.
        /// </typeparam>
        /// <typeparam name="TTarget">
        /// The fragment target type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="InjectionFragment"/>.
        /// </returns>
        public static InjectionFragment Fragment<TSource, TTarget>(Guid category)
        {
            var fragmentStub = new Mock<InjectionFragment>(MockBehavior.Loose, typeof(TSource), typeof(TTarget));
            fragmentStub.Setup(_ => _.Inherited).Returns(true);
            fragmentStub.Setup(_ => _.FragmentCategory).Returns(category);

            try
            {
                return fragmentStub.Object;
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
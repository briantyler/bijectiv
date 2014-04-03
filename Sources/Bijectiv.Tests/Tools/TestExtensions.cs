// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestExtensions.cs" company="Bijectiv">
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
//   This class contains extension methods that are useful when testing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests.Tools
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Security;

    using Microsoft.QualityTools.Testing.Fakes.Stubs;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// This class contains extension methods that are useful when testing.
    /// </summary>
    public static class TestExtensions
    {
        /// <summary>
        /// A method that does exactly nothing.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The type of <paramref name="instance"/>.
        /// </typeparam>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The original instance.
        /// </returns>
        public static TInstance Naught<TInstance>(this TInstance instance)
        {
            return instance;
        }

        public static void AssertMethodCalled<T>(this IStub<T> stub, Expression<Action<T>> expression)
            where T : class
        {
            var observer = stub.InstanceObserver as StubObserver;
            if (observer == null)
            {
                throw new ArgumentException("No InstanceObserver installed into the stub.", "stub");
            }

            var methodCall = expression.Body as MethodCallExpression;
            if (methodCall == null)
            {
                throw new ArgumentException("expression does not represent a method call.", "expression");
            }

            Assert.IsTrue(
                observer.GetCalls().Any(call => call.StubbedMethod == methodCall.Method), 
                "Method {0} was not called",
                methodCall.Method);
        }
    }
}
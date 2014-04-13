// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Spikes.cs" company="Bijectiv">
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
//   Defines the Spikes type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Tests
{
    using System.Linq;

    using Bijectiv.Builder;
    using Bijectiv.Builder.Forge;
    using Bijectiv.Tests.TestTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Spikes
    {
        [TestMethod]
        [TestCategory("Spike")]
        public void Spike_Forge_BuildsTransform()
        {
            // Arrange
            var registry = new TransformArtifactRegistry();
            var builder = new TransformStoreBuilder(registry);

            builder.Register<TestClass1, TestClass2>().Activate();

            var forge = new TransformForge(registry.Single(), registry);

            // Act
            var transform = forge.Manufacture();

            // Assert
            Assert.IsInstanceOfType(transform.Transform(new TestClass1(), new TransformContext()), typeof(TestClass2));
        }
    }
}
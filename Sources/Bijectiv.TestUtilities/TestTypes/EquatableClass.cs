// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquatableClass.cs" company="Bijectiv">
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
//   Defines the EquatableClass type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.TestUtilities.TestTypes
{
    /// <summary>
    /// A test class that overrides <see cref="object.Equals(object)"/>.
    /// </summary>
    public class EquatableClass
    {
        /// <summary>
        /// The equal value.
        /// </summary>
        private readonly int equal;

        /// <summary>
        /// The hash code.
        /// </summary>
        private readonly int hashCode;

        /// <summary>
        /// Initialises a new instance of the <see cref="EquatableClass"/> class.
        /// </summary>
        /// <param name="equal">
        /// The equal value.
        /// </param>
        /// <param name="hashCode">
        /// The hash code value.
        /// </param>
        public EquatableClass(int equal, int hashCode)
        {
            this.equal = equal;
            this.hashCode = hashCode;
        }

        /// <summary>
        /// Gets the equal value.
        /// </summary>
        public int Equal
        {
            get { return this.equal; }
        }

        /// <summary>
        /// Gets the hash code value.
        /// </summary>
        public int HashCode
        {
            get { return this.hashCode; }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current 
        /// <see cref="EquatableClass"/>.
        /// </summary>
        /// <returns>
        /// True if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// The object to compare with the current object.
        /// </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && this.Equals((EquatableClass)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="EquatableClass"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.hashCode;
        }

        /// <summary>
        /// Determines whether the specified <see cref="EquatableClass"/> is equal to the current 
        /// <see cref="EquatableClass"/>.
        /// </summary>
        /// <returns>
        /// True if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="other">
        /// The object to compare with the current object.
        /// </param>
        protected bool Equals(EquatableClass other)
        {
            return this.equal == other.equal;
        }
    }
}
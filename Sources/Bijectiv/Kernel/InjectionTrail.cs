// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectionTrail.cs" company="Bijectiv">
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
//   Defines the InjectionTrail type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.Kernel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class InjectionTrail : IInjectionTrail
    {
        private readonly ICollection<InjectionTrailItem> trail = new List<InjectionTrailItem>();
 
        private readonly ISet<object> targets = new HashSet<object>();

        public IEnumerable<object> Targets
        {
            get { return this.targets; }
        }

        public bool Add(InjectionTrailItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            this.trail.Add(item);
            return item.Target != null && this.targets.Add(item.Target);
        }

        public bool ContainsTarget(object target)
        {
            return this.targets.Contains(target);
        }

        public IEnumerator<InjectionTrailItem> GetEnumerator()
        {
            return this.trail.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
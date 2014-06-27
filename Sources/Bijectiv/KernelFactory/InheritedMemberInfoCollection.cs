// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InheritedMemberInfoCollection.cs" company="Bijectiv">
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
//   Defines the InheritedMemberInfoCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Utilities;

    public class InheritedMemberInfoCollection : ICollection<MemberInfo>
    {
        private readonly Type[] hierarchy;

        private readonly HashSet<MemberInfo> members = new HashSet<MemberInfo>();

        public InheritedMemberInfoCollection(Type type)
        {
            var types = new List<Type>();
            do
            {
                types.Add(type);
                type = type.BaseType;
            } 
            while (type != null);

            this.hierarchy = types.AsEnumerable().Reverse().ToArray();
        }

        public int Count
        {
            get { return this.members.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }


        public void Add(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (!(member is FieldInfo || member is PropertyInfo))
            {
                throw new ArgumentException(
                    string.Format("Member '{0}' is not supported: must be a field or property", member), 
                    "member");
            }

            if (!this.members.Add(member))
            {
                // The member has previously been added so there is nothing to do.
                return;
            }

            bool found = false;
            foreach (var type in this.hierarchy)
            {
                var property = type.GetProperty(member.Name, ReflectionGateway.NonPublicInstance);
                if (property == null)
                {
                    continue;
                }

                if (!member.DeclaringType.IsAssignableFrom(type))
                {
                    continue;
                }

                if (found && !property.IsOverride())
                {
                    break;
                }

                found = true;
                this.members.Add(property);

                var currentProperty = property;
                var baseProperty = currentProperty.GetBaseDefinition();
                while (baseProperty != currentProperty)
                {
                    currentProperty = baseProperty;
                    this.members.Add(baseProperty = property.GetBaseDefinition());
                }
            }
        }

        public bool Contains(MemberInfo member)
        {
            return this.members.Contains(member);
        }

        public void CopyTo(MemberInfo[] array, int arrayIndex)
        {
            this.members.CopyTo(array, arrayIndex);
        }

        public bool Remove(MemberInfo item)
        {
            throw new NotSupportedException("Items cannot be removed from this collection individually.");
        }

        public void Clear()
        {
            this.members.Clear();
        }

        public IEnumerator<MemberInfo> GetEnumerator()
        {
            return this.members.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
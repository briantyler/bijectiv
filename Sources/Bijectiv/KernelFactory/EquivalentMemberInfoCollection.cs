// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquivalentMemberInfoCollection.cs" company="Bijectiv">
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
//   Defines the EquivalentMemberInfoCollection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bijectiv.KernelFactory
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    /// <summary>
    /// A collection of <see cref="MemberInfo"/> that determines equivalence with contained members with respect to 
    /// a given type. Let the container determine equivalence with respect to type T; and let be the M1 be some member 
    /// belonging to some type. Then <see cref="EquivalentMemberInfoCollection.Contains"/> will return true when 
    /// applied to and instance t of T if and only if invoking M1 on t has exactly the same result as invoking some 
    /// member M2 that is contained within the collection on t.
    /// </summary>
    /// <remarks>
    /// Only properties and fields can be added to the collection.
    /// </remarks>
    public class EquivalentMemberInfoCollection : ICollection<MemberInfo>
    {
        /// <summary>
        /// The hierarchy of all base types below the originating type where the item at position 0 is the type 
        /// <see cref="object"/>.
        /// </summary>
        private readonly Type[] hierarchy;

        /// <summary>
        /// The members that are currently known; this set has the property that if M1 is a contained within the
        /// set and M2 has an equivalent action.
        /// </summary>
        private readonly HashSet<MemberInfo> members = new HashSet<MemberInfo>();

        /// <summary>
        /// Initialises a new instance of the <see cref="EquivalentMemberInfoCollection"/> class.
        /// </summary>
        /// <param name="type">
        /// The type that represents the limit of the inheritance hierarchy.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        public EquivalentMemberInfoCollection([NotNull] Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var types = new Collection<Type>();
            do
            {
                types.Add(type);
                // ReSharper disable once AssignNullToNotNullAttribute
                type = type.BaseType;
            } 
            while (type != null);

            this.hierarchy = types.Reverse().ToArray();
        }

        /// <summary>
        /// Gets the type that represents the limit of the inheritance hierarchy.
        /// </summary>
        public Type Type
        {
            get { return this.hierarchy.Last(); }
        }

        /// <summary>
        /// Gets the hierarchy of all base types below the originating type where the item at position 0 is the type 
        /// <see cref="object"/>.
        /// </summary>
        public IEnumerable<Type> Hierarchy
        {
            get { return this.hierarchy; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="EquivalentMemberInfoCollection"/>.
        /// </summary>
        public int Count
        {
            get { return this.members.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="EquivalentMemberInfoCollection"/> is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Adds an item to the <see cref="EquivalentMemberInfoCollection"/>.
        /// </summary>
        /// <param name="item">
        /// The <see cref="MemberInfo"/> to add to the <see cref="EquivalentMemberInfoCollection"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when any parameter is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="item"/> does not represent a field or property.
        /// </exception>
        public void Add(MemberInfo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (!(item is FieldInfo || item is PropertyInfo))
            {
                throw new ArgumentException(
                    string.Format("Member '{0}' is not supported: must be a field or property", item), 
                    "item");
            }

            this.members.AddRange(this.GetEquivalentMembers(item).Concat(item));
        }

        /// <summary>
        /// Determines whether the <see cref="EquivalentMemberInfoCollection"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="EquivalentMemberInfoCollection"/>.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="item"/> is found in the 
        /// <see cref="EquivalentMemberInfoCollection"/>.
        /// </returns>
        public bool Contains(MemberInfo item)
        {
            if (!(item is FieldInfo || item is PropertyInfo))
            {
                return false;
            }

            return this.GetEquivalentMembers(item).Any(this.members.Contains);
        }

        /// <summary>
        /// Copies the elements of the <see cref="EquivalentMemberInfoCollection"/> to an <see cref="Array"/>, starting 
        /// at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied from 
        /// <see cref="EquivalentMemberInfoCollection"/>. 
        /// The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="array"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="arrayIndex"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the number of elements in the source <see cref="EquivalentMemberInfoCollection"/> is greater 
        /// than the available space from <paramref name="arrayIndex"/> to the end of the destination 
        /// <paramref name="array"/>.
        /// </exception>
        public void CopyTo(MemberInfo[] array, int arrayIndex)
        {
            this.members.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="EquivalentMemberInfoCollection"/>.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="EquivalentMemberInfoCollection"/>.
        /// </param>
        /// <returns>
        /// A value indicating whether <paramref name="item"/> was successfully removed from the 
        /// <see cref="EquivalentMemberInfoCollection"/>. This method also returns <see langword="false"/> if 
        /// <paramref name="item"/> is not found in the original <see cref="EquivalentMemberInfoCollection"/>.
        /// </returns>
        public bool Remove(MemberInfo item)
        {
            throw new NotSupportedException("Items cannot be removed from this collection individually.");
        }

        /// <summary>
        /// Removes all items from the <see cref="EquivalentMemberInfoCollection"/>.
        /// </summary>
        public void Clear()
        {
            this.members.Clear();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A strongly typed object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<MemberInfo> GetEnumerator()
        {
            return this.members.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Gets all <see cref="MemberInfo"/> objects declared by a base class of <see cref="Type"/>.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private IEnumerable<MemberInfo> GetEquivalentMembers([NotNull] MemberInfo item)
        {
            var found = false;
            foreach (var type in this.Hierarchy)
            {
                var member = type.GetMember(item.Name, ReflectionGateway.NonPublicInstance).FirstOrDefault();
                if (member == null)
                {
                    continue;
                }

                // ReSharper disable once PossibleNullReferenceException
                if (!item.DeclaringType.IsAssignableFrom(type))
                {
                    continue;
                }

                if (found && !member.IsOverride())
                {
                    yield break;
                }

                found = true;
                yield return member;

                var currentMember = member;
                var baseMember = currentMember.GetBaseDefinition();
                while (baseMember != currentMember)
                {
                    currentMember = baseMember;
                    yield return baseMember = member.GetBaseDefinition();
                }
            }
        }
    }
}
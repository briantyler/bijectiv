﻿//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------


#pragma warning disable 1591
// ReSharper disable InconsistentNaming

namespace Bijectiv.TestUtilities.TestTypes
{
    public class MemberInfoHierarchy1: IMemberInfoHierarchy1
    {
        public int Id { get; set; }
    }

    public class MemberInfoHierarchy2 : MemberInfoHierarchy1, IMemberInfoHierarchy2
    {
        public new int Id { get; set; }
    }

    public class MemberInfoHierarchy3 : MemberInfoHierarchy2, IMemberInfoHierarchy3
    {
        public new virtual int Id { get; set; }
    }

    public class MemberInfoHierarchy4 : MemberInfoHierarchy3, IMemberInfoHierarchy4
    {
        public override int Id { get; set; }
    }

    public class MemberInfoHierarchy5 : MemberInfoHierarchy4, IMemberInfoHierarchy5
    {
        public new int Id { get; set; }
    }

    public class MemberInfoHierarchy6 : MemberInfoHierarchy5, IMemberInfoHierarchy6
    {
    }

    public interface IMemberInfoHierarchy1
    {
        int Id { get; set; }
    }

    public interface IMemberInfoHierarchy2
    {
        int Id { get; set; }
    }

    public interface IMemberInfoHierarchy3
    {
        int Id { get; set; }
    }

    public interface IMemberInfoHierarchy4
    {
        int Id { get; set; }
    }

    public interface IMemberInfoHierarchy5
    {
        int Id { get; set; }
    }

    public interface IMemberInfoHierarchy6
    {
        int Id { get; set; }
    }
}
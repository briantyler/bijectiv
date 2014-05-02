namespace Bijectiv.Tests.Factory
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Factory;

    public class Foo
    {

        public Expression Transform(TransformScaffold scaffold, Expression sourceValue, MemberInfo targetMember)
        {
            //Expression targetMemberAccess = this.CreateAccessExpression(scaffold.Target, (dynamic)targetMember);

            // I'm starting to think that the problem with merge is that it prevents the same object being mapped
            // to the same object. That is odd behaviour. I think the user has to explicitly allow merge on
            // a member by member basis.
           
            ////
            // Can write?
            //   --> YES: transform. (throw on no resolve)
            //   --> NO: do nothing. (cannot happen)
            throw new NotImplementedException();
        }
    }
}
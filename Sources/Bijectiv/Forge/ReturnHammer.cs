namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class ReturnHammer : ITransformHammer
    {
        public void Strike([NotNull] TransformOre ore)
        {
            if (ore == null)
            {
                throw new ArgumentNullException("ore");
            }

            var pop = Expression.Call(ore.ItemsExpression(), Reflect<Stack<TransformItem>>.Method(_ => _.Pop()));
            var target = Expression.Property(pop, Reflect<TransformItem>.Property(_ => _.Target));
            ore.Expressions.Add(target);
        }
    }
}
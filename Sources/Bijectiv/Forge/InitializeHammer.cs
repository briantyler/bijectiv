namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Bijectiv.Utilities;

    using JetBrains.Annotations;

    public class InitializeHammer : ITransformHammer
    {
        public void Strike([NotNull] TransformOre ore)
        {
            if (ore == null)
            {
                throw new ArgumentNullException("ore");
            }

            var ctor = Reflect<TransformItem>.Constructor(() => new TransformItem(Placeholder.Is<object>()));
            var create = Expression.New(ctor, Expression.Convert(ore.Source, typeof(object)));
            var push = Expression.Call(
                ore.ItemsExpression(),
                Reflect<Stack<TransformItem>>.Method(_ => _.Push(Placeholder.Is<TransformItem>())),
                new Expression[] { create });

            ore.Expressions.Add(push);
        }
    }
}
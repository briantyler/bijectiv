namespace Bijectiv.Builder.Forge
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bijectiv.Utilities;

    public static class TransformOreExtensions
    {
        public static Expression ItemsExpression(this TransformOre @this)
        {
            return Expression.Property(
                @this.TransformContext,
                Reflect<ITransformContext>.Property(_ => _.Items));
        }

        public static Expression PeekItemsExpression(this TransformOre @this)
        {
            return Expression.Call(
                @this.ItemsExpression(),
                Reflect<Stack<TransformItem>>.Method(_ => _.Peek()));
        }

        public static Expression CurrentSourceExpression(this TransformOre @this)
        {
            return Expression.Property(
                @this.PeekItemsExpression(),
                Reflect<TransformItem>.Property(_ => _.Source));
        }

        public static Expression CurrentTargetExpression(this TransformOre @this)
        {
            return Expression.Property(
                @this.PeekItemsExpression(), 
                Reflect<TransformItem>.Property(_ => _.Target));
        }
    }
}
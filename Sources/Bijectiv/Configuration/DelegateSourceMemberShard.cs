namespace Bijectiv.Configuration
{
    using System;
    using System.Linq;
    using System.Reflection;

    using JetBrains.Annotations;

    public class DelegateSourceMemberShard : MemberShard
    {
        private readonly Delegate @delegate;

        /// <summary>
        /// The type of the parameter to the expression.
        /// </summary>
        private readonly Type parameterType;

        private readonly Type returnType;

        public DelegateSourceMemberShard(
            [NotNull] Type source,
            [NotNull] Type target, 
            [NotNull] MemberInfo member,
            [NotNull] Delegate @delegate)
            : base(source, target, member)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            var parameters = @delegate.Method.GetParameters();
            if (parameters.Count() != 1)
            {
                throw new ArgumentException(
                    string.Format(
                    "Delegate with single parameter expected, but {0} parameters found",
                    parameters.Count()));
            }

            var parameter = parameters[0];
            //// TODO: validate the parameter type.
            
            this.parameterType = parameter.ParameterType;
            this.returnType = @delegate.Method.ReturnType;
            this.@delegate = @delegate;
        }

        public override Guid ShardCategory
        {
            get { return LegendaryShards.Source; }
        }

        public Delegate Delegate
        {
            get { return this.@delegate; }
        }

        /// <summary>
        /// Gets the type of the parameter to the delegate.
        /// </summary>
        public Type ParameterType
        {
            get { return this.parameterType; }
        }

        public Type ReturnType
        {
            get { return this.returnType; }
        }
    }
}
namespace Bijectiv.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using JetBrains.Annotations;

    public struct EnumerableFactoryRegistration : IEquatable<EnumerableFactoryRegistration>
    {
        private readonly Tuple<Type, Type> registration;

        public Type InterfaceType
        {
            get { return this.registration.Item1; }
        }

        public Type ConcreteType
        {
            get { return this.registration.Item2; }
        }

        public static bool operator ==(EnumerableFactoryRegistration left, EnumerableFactoryRegistration right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EnumerableFactoryRegistration left, EnumerableFactoryRegistration right)
        {
            return !left.Equals(right);
        }

        public bool Equals(EnumerableFactoryRegistration other)
        {
            return this.registration.Equals(other.registration);
        }

        public override bool Equals(object obj)
        {
            return obj is EnumerableFactoryRegistration && this.Equals((EnumerableFactoryRegistration)obj);
        }

        public override int GetHashCode()
        {
            return this.registration.GetHashCode();
        }

        public EnumerableFactoryRegistration([NotNull] Type interfaceType, [NotNull] Type concreteType)
        {
            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }

            if (concreteType == null)
            {
                throw new ArgumentNullException("concreteType");
            }

            if (!interfaceType.IsGenericType)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not generic.", interfaceType),
                    "interfaceType");
            }

            if (interfaceType.GetGenericArguments().Count() != 1)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not a monad.", interfaceType),
                    "interfaceType");
            }

            if (!concreteType.IsGenericType)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not generic.", concreteType),
                    "concreteType");
            }

            if (concreteType.GetGenericArguments().Count() != 1)
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is not a monad.", concreteType),
                    "concreteType");
            }

            var genericInterfaceType = interfaceType.GetGenericTypeDefinition();
            var genericConcreteType = concreteType.GetGenericTypeDefinition();

            if (!genericInterfaceType.GetInterfaces().Contains(typeof(IEnumerable<>)))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is does not implement IEnumerable<>.", genericInterfaceType),
                    "interfaceType");
            }

            if (!genericConcreteType.GetInterfaces().Contains(typeof(ICollection<>)))
            {
                throw new ArgumentException(
                    string.Format("Type '{0}' is does not implement ICollection<>.", genericConcreteType),
                    "concreteType");
            }

            if (!genericInterfaceType.IsAssignableFrom(genericConcreteType))
            {
                
            }

            this.registration = Tuple.Create(genericInterfaceType, genericConcreteType);
        }
    }
}
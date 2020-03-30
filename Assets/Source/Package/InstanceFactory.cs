using System;
using System.Collections.Generic;

namespace Omega.Package
{
    public abstract class InstanceFactory
    {
        private static Dictionary<Type, InstanceFactory> _factories = new Dictionary<Type, InstanceFactory>(7);

        public static InstanceFactory GetFactory(Type type)
        {
            if(type is null)
                throw new ArgumentNullException(nameof(type));
        
            if (!_factories.TryGetValue(type, out var factory))
            {
                if (type.IsValueType)
                {
                    var factoryTypeDefinition = typeof(StructFactory<>);
                    var factoryType = factoryTypeDefinition.MakeGenericType(type);
                    factory = (InstanceFactory) Activator.CreateInstance(factoryType);
                }
                else
                {
                    factory = new ActivatorFactory(type);
                }

                _factories.Add(type, factory);
            }

            return factory;
        }

        public static object Create(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return GetFactory(type).Create();
        }
        
        public abstract object Create();

        private sealed class StructFactory<T> : InstanceFactory
            where T : struct
        {
            public override object Create()
                => CreateNonBoxing();

            /* ----------------------------------------------------------------------------- */
            /* new T() in runtime call Activator.CreateInstance<T>(), but default(T) no call */
            /* ----------------------------------------------------------------------------- */
            public T CreateNonBoxing() => default(T);
        }

        private sealed class ActivatorFactory : InstanceFactory
        {
            private Type _type;

            public ActivatorFactory(Type type)
            {
                _type = type;
            }

            public override object Create()
            {
                return Activator.CreateInstance(_type);
            }
        }
    }
}
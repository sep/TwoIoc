using System;
using Net35;
using TwoIoc.Extensions;

namespace TwoIoc.Builders
{
    public class ConcreteInstanceBuilder : IBuilder
    {
        readonly Container _resolver;
        readonly Type _concreteType;

        public ConcreteInstanceBuilder(Container resolver, Type concreteType)
        {
            _resolver = resolver;
            _concreteType = concreteType;
        }

        public object Build()
        {
            var ctor = _concreteType.GetConstructors().MaxItem(c => c.GetParameters().Length);

            var ctorArgs = ctor.GetParameters().Select(p => Resolve(p.ParameterType));
            return Activator.CreateInstance(_concreteType, ctorArgs.ToArray());
        }

        private object Resolve(Type parameterType)
        {
            return _resolver.Get(parameterType);
        }
    }
}
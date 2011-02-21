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

        public object Build(object[] args)
        {
            var ctor = _concreteType.GetConstructors().MaxItem(c => c.GetParameters().Length);

            var ctorArgs = ctor.GetParameters().Select(p => Resolve(p.ParameterType, args));
            return Activator.CreateInstance(_concreteType, ctorArgs.ToArray());
        }

        private object Resolve(Type parameterType, object[] args)
        {
            var result = args.FirstOrDefault(x => parameterType.IsAssignableFrom(x.GetType()));

            if (result != null)
                return result;

            return _resolver.Get(parameterType);
        }
    }
}
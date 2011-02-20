using System;
using System.Collections.Generic;
using Net35;
using TwoIoc.Extensions;

namespace TwoIoc.Builders
{
    public class GreediestCtorBuilder : IBuilder
    {
        readonly Type _concreteType;
        readonly IDictionary<string, object> _ctorValues;

        public GreediestCtorBuilder(Type concreteType, IDictionary<string, object> ctorValues)
        {
            _concreteType = concreteType;
            _ctorValues = ctorValues;
        }

        public object Build()
        {
            var ctor = _concreteType.GetConstructors().MaxItem(c => c.GetParameters().Length);

            return Activator.CreateInstance(_concreteType, ctor.GetParameters().Select(p => _ctorValues[p.Name]).ToArray());
        }
    }
}
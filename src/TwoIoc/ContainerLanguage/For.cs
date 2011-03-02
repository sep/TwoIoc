using System;
using TwoIoc.Extensions;

namespace TwoIoc.ContainerLanguage
{
    public class For
    {
        Container _container;

        public For(Container container)
        {
            _container = container;
        }

        public void UseInstance<T>(object objectToUse)
        {
            _container.RegisterInstance(typeof(T), objectToUse);
        }

        public void Use<T>()
        {
            _container.RegisterType(typeof(T), typeof(T));
        }

        public void UseWithCtorParams<T>(object ctorParamsAnonObj)
        {
            _container.RegisterType(typeof(T), typeof(T), ctorParamsAnonObj.PropertyValuesToDictionary());
        }
    }

    public class For<T>
    {
        readonly Container _container;

        public For(Container container)
        {
            _container = container;
        }

        public void UseInstance(T objectToUse)
        {
            _container.RegisterInstance(typeof(T), objectToUse);
        }

        public void Use<TConcrete>() where TConcrete : T
        {
            _container.RegisterType(typeof(T), typeof(TConcrete));
        }

        public void Use<TConcrete>(object ctorParamsAnonObj) where TConcrete : T
        {
            _container.RegisterType(typeof(T), typeof(TConcrete), ctorParamsAnonObj.PropertyValuesToDictionary());
        }
    }
}
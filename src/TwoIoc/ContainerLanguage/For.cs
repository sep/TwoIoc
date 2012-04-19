using Net35;
using TwoIoc.Builders;
using TwoIoc.Extensions;

namespace TwoIoc.ContainerLanguage
{
    public class For
    {
        readonly Container _container;

        public For(Container container)
        {
            _container = container;
        }

        public void UseInstance<T>(object objectToUse)
        {
            _container.RegisterInstance(typeof(T), objectToUse);
        }

        public Option Use<T>()
        {
            return new Option(_container.RegisterType(typeof(T), typeof(T)));
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

        public Option UseFunc<TConcrete>(Func<TConcrete> buildIt) where TConcrete : T
        {
            return new Option(_container.RegisterType(typeof(T), new FuncBuilder<TConcrete>(buildIt)));
        }

        public Option UseInstance(T objectToUse)
        {
            return new Option(_container.RegisterInstance(typeof(T), objectToUse));
        }

        public Option Use<TConcrete>() where TConcrete : T
        {
            return new Option(_container.RegisterType(typeof(T), typeof(TConcrete)));
        }

        public Option Use<TConcrete>(object ctorParamsAnonObj) where TConcrete : T
        {
            return new Option(_container.RegisterType(typeof(T), typeof(TConcrete), ctorParamsAnonObj.PropertyValuesToDictionary()));
        }
    }
}
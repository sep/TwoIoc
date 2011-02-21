using System;
using System.Collections.Generic;
using Net35;
using TwoIoc.Builders;
using TwoIoc.ContainerLanguage;

namespace TwoIoc
{
    public class Container
    {
        private readonly Dictionary<Type, IBuilder> _registrations = new Dictionary<Type, IBuilder>();

        public Container()
        {
            _registrations.Add(typeof (Container), new GivenInstanceBuilder(this));
        }

        public For<T> For<T>()
        {
            return new For<T>(this);
        }

        public For Concrete { get { return new For(this);}}

        public T Get<T>(params object[] args)
        {
            return (T) Get(typeof (T), args);
        }

        public object Get(Type toResolve, params object[] args)
        {
            return _registrations[toResolve].Build(args);
        }

        internal void RegisterInstance(Type type, object objectToUse)
        {
            if (!_registrations.ContainsKey(type))
                _registrations.Add(type, new GivenInstanceBuilder(objectToUse));
        }

        public void RegisterType(Type typeRegistration, Type typeInstance)
        {
            RegisterType(typeRegistration, new ConcreteInstanceBuilder(this, typeInstance));
        }

        internal void RegisterType(Type typeRegistration, Type typeInstance, IDictionary<string, object> ctorValues)
        {
            RegisterType(typeRegistration, new GreediestCtorBuilder(typeInstance, ctorValues));
        }

        internal void RegisterType(Type typeRegistration, IBuilder builder)
        {
            _registrations.Add(typeRegistration, builder);
        }

        public void SetupConvention(Action<ConventionSetup> action)
        {
            var setup = new ConventionSetup();
            action(setup);

            foreach (var type in setup.Assemblies
                .SelectMany(x => x.GetTypes())
                .Where(t => t.IsPublic || setup.IncludeNonPublicTypes)
                .Where(setup.Where))
                setup.Do(type, this);
        }

        public bool Has<T>()
        {
            return _registrations.ContainsKey(typeof (T));
        }
    }
}

using System;
using System.Collections.Generic;
using Net35;
using TwoIoc.Builders;
using TwoIoc.ContainerLanguage;

namespace TwoIoc
{
    public class Registration
    {
        public IBuilder Builder { get; set; }
    }

    public class Container
    {
        private readonly Dictionary<Type, Registration> _registrations = new Dictionary<Type, Registration>();

        public Container()
        {
            _registrations.Add(typeof (Container), new Registration{Builder =  new GivenInstanceBuilder(this)});
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
            if(!_registrations.ContainsKey(toResolve))
                throw new ContainerResolutionException(string.Format("Type not registered: {0}", toResolve));

            return _registrations[toResolve].Builder.Build(args);
        }

        internal Registration RegisterInstance(Type type, object objectToUse)
        {
            return RegisterType(type, new GivenInstanceBuilder(objectToUse));
        }

        public Registration RegisterType(Type typeRegistration, Type typeInstance)
        {
            return RegisterType(typeRegistration, new ConcreteInstanceBuilder(this, typeInstance));
        }

        internal Registration RegisterType(Type typeRegistration, Type typeInstance, IDictionary<string, object> ctorValues)
        {
            return RegisterType(typeRegistration, new GreediestCtorBuilder(typeInstance, ctorValues));
        }

        internal Registration RegisterType(Type typeRegistration, IBuilder builder)
        {
            if (!_registrations.ContainsKey(typeRegistration))
                _registrations.Add(typeRegistration, new Registration{Builder =  builder});
            return _registrations[typeRegistration];
        }

        public void Eject<T>()
        {
            _registrations.Remove(typeof(T));
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

    public class ContainerResolutionException : Exception
    {
        public ContainerResolutionException(string message) : base(message)
        {
        }
    }
}

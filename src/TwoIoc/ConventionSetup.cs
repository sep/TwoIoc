using System;
using System.Collections.Generic;
using System.Reflection;
using Net35;

namespace TwoIoc
{
    public class ConventionSetup
    {
        private readonly List<Assembly> _assemblies;

        public IEnumerable<Assembly> Assemblies { get { return _assemblies; } }
        public Func<Type, bool> Where { get; set; }
        public Action<Type, Container> Do { get; set; }
        public bool IncludeNonPublicTypes { get; set; }

        public ConventionSetup()
        {
            _assemblies = new List<Assembly>();
            Where = _ => { return false; };
            Do = (_, __) => { };
        }

        public void AddAssembly(string assembly)
        {
            _assemblies.Add(Assembly.Load(assembly));
        }

        public void AddAssemblyContainingType<T>()
        {
            _assemblies.Add(typeof (T).Assembly);
        }

        public void AddAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }
    }
}
using Machine.Specifications;

namespace TwoIoc.Tests
{
    [Subject("Autowire")]
    public class AutowiresASingleLayerDependency
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.For<ClassWithCtorDependency>().Use<ClassWithCtorDependency>();
            Container.For<TestClass>().Use<TestClass>();
        };

        It can_resolve_an_instance = () =>
        {
            var resolved = Container.Get<ClassWithCtorDependency>();

            resolved.ShouldBe(typeof(ClassWithCtorDependency));
            resolved.Dependency.ShouldNotBeNull();
        };
    }

    [Subject("Autowire")]
    public class AutowiresAMultiLayerDependency
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.For<ClassWithMultiLayerDependency>().Use<ClassWithMultiLayerDependency>();
            Container.For<ClassWithCtorDependency>().Use<ClassWithCtorDependency>();
            Container.For<TestClass>().Use<TestClass>();
        };

        It can_resolve_an_instance = () =>
        {
            var resolved = Container.Get<ClassWithMultiLayerDependency>();

            resolved.ShouldBe(typeof(ClassWithMultiLayerDependency));
            resolved.Dependency.ShouldNotBeNull();
            resolved.Dependency.Dependency.ShouldNotBeNull();
        };
    }

    class ClassWithMultiLayerDependency
    {
        public ClassWithCtorDependency Dependency { get; set; }

        public ClassWithMultiLayerDependency(ClassWithCtorDependency dependency)
        {
            Dependency = dependency;
        }
    }
}
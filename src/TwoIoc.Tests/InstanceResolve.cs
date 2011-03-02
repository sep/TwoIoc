using Machine.Specifications;

namespace TwoIoc.Tests
{
    [Subject("Instance Resolution")]
    public class InstanceResolveWithOnlyOneRegisteredInstance
    {
        static TestClass Expected;
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
            Expected = new TestClass();
        };

        Because of = () =>
        {
            Container.For<TestClass>().UseInstance(Expected);
        };

        It resolves_the_given_instance = () =>
        {
            Container.Get<TestClass>().ShouldEqual(Expected);
        };
    }

    [Subject("Instance Resolution")]
    public class InstanceResolveWithMultipleInstancesOfDifferentTypes
    {
        static TestClass Expected;
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
            Expected = new TestClass();
        };

        Because of = () =>
        {
            Container.For<TestClass>().UseInstance(Expected);
            Container.For<WhateverObject>().UseInstance(new WhateverObject());
        };

        It resolves_the_given_instance = () =>
        {
            Container.Get<TestClass>().ShouldEqual(Expected);
        };
    }

    [Subject("Instance Resolution")]
    public class InstanceResolvedWithMultipleInstancesOfTheSameType
    {
        static TestClass Expected;
        static TestClass Other;
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
            Expected = new TestClass();
            Other = new TestClass();
        };

        Because of = () =>
        {
            Container.For<TestClass>().UseInstance(Expected);
            Container.For<TestClass>().UseInstance(Other);
        };

        It uses_the_first = () =>
        {
            Container.Get<TestClass>().ShouldEqual(Expected);
        };
    }

    [Subject("Instance Resolution")]
    public class InstanceResolvedWithAdditionalArguments
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.Concrete.Use<ClassWithCtorDependency>();
        };

        It can_resolve_with_additional_help = () =>
        {
            var resolved = Container.Get<ClassWithCtorDependency>(new TestClass());
            
            resolved.Dependency.ShouldNotBeNull();
        };
    }

    [Subject("Instance Resolution")]
    public class InstanceResolvedWithAdditionalSubclassedArguments
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.Concrete.Use<ClassWithParentDependency>();
        };

        It can_resolve_with_additional_help = () =>
        {
            var resolved = Container.Get<ClassWithParentDependency>(new TestClass());

            resolved.Dependency.ShouldNotBeNull();
        };
    }

    internal class TestClassParent { }
    internal class TestClass : TestClassParent { }

    internal class WhateverObject { }

    internal class ClassWithParentDependency
    {
        public TestClassParent Dependency { get; set; }

        public ClassWithParentDependency(TestClassParent dependency)
        {
            Dependency = dependency;
        }
    }
}
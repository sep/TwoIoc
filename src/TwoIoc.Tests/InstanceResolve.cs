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
            Container.For<TestClass>().Use(Expected);
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
            Container.For<TestClass>().Use(Expected);
            Container.For<WhateverObject>().Use(new WhateverObject());
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
            Container.For<TestClass>().Use(Expected);
            Container.For<TestClass>().Use(Other);
        };

        It uses_the_first = () =>
        {
            Container.Get<TestClass>().ShouldEqual(Expected);
        };
    }

    internal class TestClassParent { }
    internal class TestClass : TestClassParent { }

    internal class WhateverObject { }
}
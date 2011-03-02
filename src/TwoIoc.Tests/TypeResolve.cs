using Machine.Specifications;

namespace TwoIoc.Tests
{
    [Subject("Registration")]
    public class EjectionEjectsRegistrationsForType
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
            Container.For<string>().UseInstance("hello world");
        };

        Because of = () =>
        {
            Container.Eject<string>();
        };

        It throws_duplicate_registration_exception = () =>
        {
            Catch.Exception(() => Container.Get<string>()).ShouldNotBeNull();
        };
    }

    [Subject("Type Resolution")]
    public class TypeResolveWithSameTypeDefaultCtor
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.Concrete.Use<TestClass>();
        };
    
        It can_resolve_an_instance = () =>
        {
            Container.Get<TestClass>().ShouldBe(typeof(TestClass));
        };
    }

    [Subject("Type Resolution")]
    public class TypeResolveFromParentTypeDefaultCtor
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.For<TestClassParent>().Use<TestClass>();
        };

        It can_resolve_an_instance = () =>
        {
            Container.Get<TestClassParent>().ShouldBe(typeof(TestClass));
        };
    }

    [Subject("Type Resolution")]
    public class TypeResolveFromSameTypeWithCtorValuesSpecified
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.Concrete.UseWithCtorParams<ClassWithCtorParams>(new { value1 = "yohoho", value2 = "and a bottle of rum", value3 = 3 });
        };

        It can_resolve_an_instance = () =>
        {
            var resolved = Container.Get<ClassWithCtorParams>();

            resolved.ShouldBe(typeof(ClassWithCtorParams));
            resolved.Value1.ShouldEqual("yohoho");
            resolved.Value2.ShouldEqual("and a bottle of rum");
            resolved.Value3.ShouldEqual(3);
        };
    }

    [Subject("Type Resolution")]
    public class TypeResolveFromSameTypeUsesGreediestCtor
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.Concrete.UseWithCtorParams<ClassWithMultipleCtors>(new { value1 = "yohoho", value2 = "and a bottle of rum"});
        };

        It can_resolve_an_instance = () =>
        {
            var resolved = Container.Get<ClassWithMultipleCtors>();

            resolved.ShouldBe(typeof(ClassWithMultipleCtors));
            resolved.Value1.ShouldEqual("yohoho");
            resolved.Value2.ShouldEqual("and a bottle of rum");
            resolved.Value3.ShouldBeNull();
        };
    }

    [Subject("Type Resolution")]
    public class TypeResolveFromSameTypeUsingSpecifiedCtor
    {
        
    }

    class ClassWithCtorParams
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public int Value3 { get; set; }

        public ClassWithCtorParams(string value1, string value2, int value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
    }

    class ClassWithMultipleCtors
    {
        public string Value3 { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public ClassWithMultipleCtors(string value3)
        {
            Value3 = value3;
        }
    
        public ClassWithMultipleCtors(string value1, string value2)
        {
            Value1 = value1;
            Value2 = value2;
        }
    }

    class ClassWithCtorDependency
    {
        public TestClass Dependency { get; set; }

        public ClassWithCtorDependency(TestClass dependency)
        {
            Dependency = dependency;
        }
    }
}
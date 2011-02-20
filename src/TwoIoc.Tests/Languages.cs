using Machine.Specifications;

namespace TwoIoc.Tests
{
    [Subject("Registration Language")]
    public class RegistrationLanguagesForcesUseToBeSubclassOfFor
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            //Container.For<TestObject>().Use(new WhateverObject());
        };

        It does_not_compile = () =>
        {
            // the above 'because of' is commented out because it does not compile, which is the point of this test.
        };
    }
}
using Machine.Specifications;
using TestAssembly1;
using TestAssembly2;

namespace TwoIoc.Tests
{
    [Subject("Conventions")]
    public class ExecutesConvention
    {
        static Container Container;

        Establish context = () =>
        {
            Container = new Container();
        };

        Because of = () =>
        {
            Container.SetupConvention(x =>
            {
                x.AddAssembly("TestAssembly1");
                x.Where = type => !type.Name.EndsWith("Filtered");
                x.Do = (type, container) => container.RegisterType(type, type);
            });
        };

        It scans_given_assemblies = () =>
        {
            Container.Has<FromTestAssembly1>().ShouldBeTrue();
            Container.Has<FromTestAssembly2>().ShouldBeFalse();
        };

        It filters_types_appropriately = () =>
        {
            Container.Has<FromTestAssembly1Filtered>().ShouldBeFalse();
        };

        It registers_types_correctly = () =>
        {
            Container.Get<FromTestAssembly1>().ShouldBe(typeof(FromTestAssembly1));
        };
    }
}
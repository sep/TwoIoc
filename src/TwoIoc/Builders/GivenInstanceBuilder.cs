namespace TwoIoc.Builders
{
    public class GivenInstanceBuilder : IBuilder
    {
        readonly object _instance;

        public GivenInstanceBuilder(object instance)
        {
            _instance = instance;
        }

        public object Build(object[] args)
        {
            return _instance;
        }
    }
}
namespace TwoIoc.Builders
{
    public class SingletonBuilder : IBuilder
    {
        readonly IBuilder _builder;
        object _built;

        public SingletonBuilder(IBuilder builder)
        {
            _builder = builder;
        }

        public object Build(object[] args)
        {
            return _built ?? (_built = _builder.Build(args));
        }
    }
}
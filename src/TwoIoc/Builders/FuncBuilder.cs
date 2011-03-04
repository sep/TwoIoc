using Net35;

namespace TwoIoc.Builders
{
    public class FuncBuilder<T> : IBuilder
    {
        readonly Func<T> _builder;

        public FuncBuilder(Func<T> builder)
        {
            _builder = builder;
        }

        public object Build(object[] args)
        {
            return _builder();
        }
    }
}
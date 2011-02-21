namespace TwoIoc.Builders
{
    public interface IBuilder
    {
        object Build(object[] args);
    }
}
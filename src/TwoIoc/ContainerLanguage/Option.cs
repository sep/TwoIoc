using TwoIoc.Builders;

namespace TwoIoc.ContainerLanguage
{
    public class Option
    {
        private readonly Registration _reg;

        public Option(Registration reg)
        {
            _reg = reg;
        }

        public Option AsSingleton()
        {
            _reg.Builder = new SingletonBuilder(_reg.Builder);
            return this;
        }
    }
}
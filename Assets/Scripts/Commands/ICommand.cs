namespace Commands
{
    public interface ICommand
    {
        public void Do(object arg = null);
    }

    public abstract class Command : ICommand
    {
        public void Do(object arg = null)
        {
            Handle();
        }
        
        protected abstract void Handle();
    }
    public abstract class Command<T> : ICommand
    {
        public void Do(object arg)
        {
            Handle((T)arg);
        }

        protected abstract void Handle(T arg);
    }
}
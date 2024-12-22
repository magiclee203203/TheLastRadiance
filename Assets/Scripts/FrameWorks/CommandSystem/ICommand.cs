public interface ICommand
{
    void Execute<T>(T receiver);
}


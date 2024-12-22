public class EnableNPCCommand : ICommand
{
    public void Execute<T>(T receiver)
    {
        GameStateManager.Instance.EnableNPC();
    }
}
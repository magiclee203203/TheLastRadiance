public class ShowIntroPanelCommand : ICommand
{
    public void Execute<T>(T receiver)
    {
        if (receiver is not UIManager manager) return;

        manager.ShowIntroPanel();
    }
}
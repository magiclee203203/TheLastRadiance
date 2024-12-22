public class NPCIsActiveCondition : ICondition
{
    public bool IsConditionMet()
    {
        return GameStateManager.Instance.IsNPCActive;
    }
}
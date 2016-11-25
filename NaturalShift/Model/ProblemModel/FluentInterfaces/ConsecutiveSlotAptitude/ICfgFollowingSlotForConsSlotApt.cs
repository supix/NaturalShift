namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgFollowingSlotForConsSlotApt
    {
        IConfigurableProblem IsFollowedBySlot(int slot);
        ICfgMultipleFollowingSlotForConsSlotApt IsFollowedBySlots();
        IConfigurableProblem IsFollowedByAnySlot();
    }
}
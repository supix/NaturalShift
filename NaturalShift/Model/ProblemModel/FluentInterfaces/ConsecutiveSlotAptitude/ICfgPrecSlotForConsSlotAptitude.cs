namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgPrecSlotForConsSlotAptitude
    {
        ICfgFollowingSlotForConsSlotApt WhenSlot(int slot);

        ICfgMultiplePrecSlotForConsSlotApt WhenSlots();

        ICfgFollowingSlotForConsSlotApt WhenAnySlot();
    }
}
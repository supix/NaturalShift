namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgSlotForAptitude
    {
        ICfgDayForAptitude ForAllSlots();

        ICfgDayForAptitude ForSlot(int slot);

        ICfgMultipleSlotsForAptitude ForSlots();
    }
}
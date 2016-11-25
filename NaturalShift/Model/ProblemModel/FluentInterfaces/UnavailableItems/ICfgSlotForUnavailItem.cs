namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgSlotForUnavailItem
    {
        ICfgDayForUnavailItem UnavailableForSlot(int slot);

        ICfgDayForUnavailItem UnavailableForAllSlots();

        ICfgMultipleSlotsForUnavailItem UnavailableForSlots();
    }
}
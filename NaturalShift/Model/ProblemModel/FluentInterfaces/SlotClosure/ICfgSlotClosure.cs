namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgSlotClosure
    {
        ICfgDayOfClosure Slot(int slot);

        ICfgDayOfClosure AllSlots();

        ICfgMultipleSlotClosure Slots();
    }
}
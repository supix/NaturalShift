namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgWeight
    {
        IConfigurableProblem ToSlot(int slot);

        ICfgMultipleSlotsForSlotWeight ToSlots();

        IConfigurableProblem ToAllSlots();

        IConfigurableProblem ToItem(int item);

        ICfgMultipleItemsForItemWeight ToItems();

        IConfigurableProblem ToAllItems();
    }
}
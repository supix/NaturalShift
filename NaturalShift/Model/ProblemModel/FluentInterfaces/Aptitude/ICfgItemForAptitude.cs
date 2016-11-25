namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgItemForAptitude
    {
        ICfgSlotForAptitude ToAllItems();

        ICfgSlotForAptitude ToItem(int item);

        ICfgMultipleItemsForAptitude ToItems();
    }
}
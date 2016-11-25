namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgItemForUnavailItem
    {
        ICfgSlotForUnavailItem Item(int item);

        ICfgSlotForUnavailItem AllItems();

        ICfgMultipleItemsForUnavailItem Items();
    }
}
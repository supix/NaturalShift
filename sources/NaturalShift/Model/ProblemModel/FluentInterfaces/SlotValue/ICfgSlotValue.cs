namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgSlotValue
    {
        IConfigurableProblem ToSlot(int slot);

        ICfgMultipleSlotsForSlotValue ToSlots();

        IConfigurableProblem ToAllSlots();
    }
}
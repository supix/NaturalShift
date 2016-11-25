namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgSlotValue
    {
        IConfigurableProblem ToSlot(int slot);
    }
}
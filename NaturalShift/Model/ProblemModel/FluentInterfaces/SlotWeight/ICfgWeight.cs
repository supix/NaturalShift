namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgWeight
    {
        IConfigurableProblem ToSlot(int slot);

        IConfigurableProblem ToItem(int item);
    }
}
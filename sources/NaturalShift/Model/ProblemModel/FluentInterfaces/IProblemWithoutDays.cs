namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface IProblemWithoutDays
    {
        IProblemWithoutSlots WithDays(int days);
    }
}
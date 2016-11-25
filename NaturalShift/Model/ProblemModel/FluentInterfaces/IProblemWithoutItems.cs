namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface IProblemWithoutItems
    {
        IConfigurableProblem WithItems(int items);
    }
}
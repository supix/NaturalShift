namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface IProblemWithoutSlots
    {
        IProblemWithoutItems WithSlots(int slots);
    }
}
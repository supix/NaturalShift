namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgDayOfClosure
    {
        IConfigurableProblem InDay(int day);

        IConfigurableProblem Always();

        ICfgMultipleDaysOfClosure InDays();
    }
}
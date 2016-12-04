namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgDayForAptitude
    {
        IConfigurableProblem InDay(int day);

        IConfigurableProblem Always();

        ICfgMultipleDaysForAptitude InDays();
    }
}
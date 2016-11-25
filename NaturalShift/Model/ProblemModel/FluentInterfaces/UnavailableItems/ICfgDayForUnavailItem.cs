namespace NaturalShift.Model.ProblemModel.FluentInterfaces
{
    public interface ICfgDayForUnavailItem
    {
        IConfigurableProblem Always();

        IConfigurableProblem InDay(int day);

        ICfgMultipleDaysForUnavailItem InDays();
    }
}
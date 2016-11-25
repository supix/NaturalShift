namespace NaturalShift.SolvingEnvironment
{
    public interface IConfiguringTime
    {
        IConfigurableSolvingEnvironment Milliseconds { get; }
        IConfigurableSolvingEnvironment Seconds { get; }
        IConfigurableSolvingEnvironment Minutes { get; }
    }
}
using NaturalShift.Model.ProblemModel;

namespace NaturalShift.SolvingEnvironment
{
    public interface ISolvingEnvironmentWithoutProblem
    {
        IConfigurableSolvingEnvironment ForProblem(Problem problem);
    }
}
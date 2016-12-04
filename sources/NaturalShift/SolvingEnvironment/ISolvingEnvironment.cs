using NaturalShift.Model.SolutionModel;

namespace NaturalShift.SolvingEnvironment
{
    public interface ISolvingEnvironment
    {
        ISolution Solve();
    }
}
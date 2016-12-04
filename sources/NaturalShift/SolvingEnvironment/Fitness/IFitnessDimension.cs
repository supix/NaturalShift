using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal interface IFitnessDimension
    {
        float Evaluate(ShiftMatrix matrix);
    }
}
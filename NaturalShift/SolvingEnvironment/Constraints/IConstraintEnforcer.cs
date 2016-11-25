using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal interface IConstraintEnforcer
    {
        void EnforceConstraint(ShiftMatrix matrix, int day, int slot);
    }
}
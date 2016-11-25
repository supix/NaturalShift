using NaturalShift.SolvingEnvironment.Matrix;
using System;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class CrossItemAptitudeEnforcer : IConstraintEnforcer
    {
        private readonly float[,,,] multipliers; //indexes are slot1, slot2, item1, item2

        public CrossItemAptitudeEnforcer(Single[,,,] multipliers)
        {
            this.multipliers = multipliers;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                int item = matrix[day, slot].ChosenItem.Value;
                for (int s = 0; s < matrix.Slots; s++)
                    for (int i = 0; i < matrix.Items; i++)
                    {
                        if ((item != i) && (slot != s))
                        {
                            var all = matrix[day, s];
                            if (!all.Processed && multipliers[slot, s, item, i] != 1)
                            {
                                all.CurrentAptitudes[i] =
                                    all.CurrentAptitudes[i]
                                    * multipliers[slot, s, item, i];
                            }
                        }
                    }
            }
        }
    }
}
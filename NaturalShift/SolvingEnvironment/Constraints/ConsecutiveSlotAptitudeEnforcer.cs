using NaturalShift.SolvingEnvironment.Matrix;
using System;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class ConsecutiveSlotAptitudeEnforcer : IConstraintEnforcer
    {
        private readonly Single[,] multipliers;
        private readonly int[] slotLengths;

        public ConsecutiveSlotAptitudeEnforcer(Single[,] multipliers, int[] slotLengths)
        {
            this.multipliers = multipliers;
            this.slotLengths = slotLengths;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            int? chosenItem = matrix[day, slot].ChosenItem;
            if (chosenItem.HasValue)
            {
                int nextDayToEnforce = day + slotLengths[slot];
                if (nextDayToEnforce < matrix.Days)
                {
                    for (int s = 0; s < matrix.Slots; s++)
                    {
                        var all = matrix[nextDayToEnforce, s];
                        if (!all.Processed)
                        {
                            all.CurrentAptitudes[chosenItem.Value] =
                                all.CurrentAptitudes[chosenItem.Value]
                                * multipliers[slot, s];
                        }
                    }
                }
            }
        }
    }
}
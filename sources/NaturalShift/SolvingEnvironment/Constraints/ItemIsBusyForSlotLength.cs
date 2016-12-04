using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class ItemIsBusyForSlotLength : IConstraintEnforcer
    {
        private readonly int[] lengths;

        public ItemIsBusyForSlotLength(int[] lengths)
        {
            this.lengths = lengths;
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                var untilDay = day + lengths[slot] - 1;
                if (untilDay >= matrix.Days)
                    untilDay = matrix.Days - 1;

                for (int d = day + 1; d <= untilDay; d++)
                    for (int s = 0; s < matrix.Slots; s++)
                    {
                        var all = matrix[d, s];
                        if (!all.Processed)
                            all.CurrentAptitudes[matrix[day, slot].ChosenItem.Value] = 0;
                    }
            }
        }
    }
}
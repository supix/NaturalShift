using NaturalShift.SolvingEnvironment.Matrix;

namespace NaturalShift.SolvingEnvironment.Constraints
{
    internal class NoUbiquityOverIncompatibleSlots : IConstraintEnforcer
    {
        private readonly bool[,] compatibleSlots;
        private readonly int[] slotLengths;

        public NoUbiquityOverIncompatibleSlots(bool[,] compatibleSlots, int[] slotLengths, int slots)
        {
            this.compatibleSlots = compatibleSlots;
            this.slotLengths = slotLengths;

            if (this.compatibleSlots == null)
                this.compatibleSlots = new bool[slots, slots];

            if (this.slotLengths == null)
            {
                this.slotLengths = new int[slots];
                for (int i = 0; i < slots; i++)
                    this.slotLengths[i] = 1;
            }
        }

        public void EnforceConstraint(ShiftMatrix matrix, int day, int slot)
        {
            if (matrix[day, slot].ChosenItem.HasValue)
            {
                var chosenItem = matrix[day, slot].ChosenItem.Value;
                for (int d = day; d < day + slotLengths[slot]; d++)
                {
                    for (int s = 0; s < matrix.Slots; s++)
                    {
                        var all = matrix[day, s];
                        if ((!all.Processed) && (!compatibleSlots[slot, s]))
                            all.CurrentAptitudes[chosenItem] = 0;
                    }
                }
            }
        }
    }
}
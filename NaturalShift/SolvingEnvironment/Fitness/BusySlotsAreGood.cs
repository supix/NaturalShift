using NaturalShift.SolvingEnvironment.Matrix;
using System;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class BusySlotsAreGood : IFitnessDimension
    {
        private readonly Single[] slotValues;

        public BusySlotsAreGood(Single[] slotValues)
        {
            this.slotValues = slotValues;
        }

        public float Evaluate(ShiftMatrix matrix)
        {
            Single value = 0;
            Single maxValue = 0;
            for (int day = 0; day < matrix.Days; day++)
                for (int slot = 0; slot < matrix.Slots; slot++)
                {
                    var all = matrix[day, slot];
                    if (!all.Forced)
                    {
                        maxValue += (slotValues != null ? slotValues[slot] : 1);
                        if (all.ChosenItem.HasValue)
                            value += (slotValues != null ? slotValues[slot] : 1);
                    }
                }

            return value / maxValue;
        }
    }
}
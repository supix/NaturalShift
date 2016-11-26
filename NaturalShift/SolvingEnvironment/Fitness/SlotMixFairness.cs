using NaturalShift.SolvingEnvironment.Matrix;
using NaturalShift.SolvingEnvironment.Utils;
using System;
using System.Diagnostics;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class SlotMixFairness : IFitnessDimension
    {
        private readonly int[] slotItemOccurrencies;

        public SlotMixFairness(int items)
        {
            slotItemOccurrencies = new int[items];
        }

        public float Evaluate(ShiftMatrix matrix)
        {
            for (int item = 0; item < matrix.Items; item++)
                slotItemOccurrencies[item] = 0;

            Single value = 0;
            for (int slot = 0; slot < matrix.Slots; slot++)
            {
                for (int day = 0; day < matrix.Days; day++)
                {
                    var all = matrix[day, slot];
                    if (all.ChosenItem.HasValue)
                    {
                        slotItemOccurrencies[all.ChosenItem.Value]++;
                    }
                }
                value += Utils.MathUtils.StandardDeviation(slotItemOccurrencies);
                for (int item = 0; item < matrix.Items; item++)
                    slotItemOccurrencies[item] = 0;
            }
            var result = 1 - (value / (matrix.Slots * matrix.Items));

            Debug.Assert(result >= 0 && result <= 1);

            return result;
        }
    }
}
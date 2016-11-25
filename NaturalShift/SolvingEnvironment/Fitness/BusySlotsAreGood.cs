using NaturalShift.SolvingEnvironment.Matrix;
using System;
using System.Diagnostics;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class BusySlotsAreGood : IFitnessDimension
    {
        private readonly Single[] slotValues;
        private ShiftMatrix lastMatrix = null;
        private Single lastNormalizer;

        public BusySlotsAreGood(Single[] slotValues)
        {
            this.slotValues = slotValues;
        }

        public float Evaluate(ShiftMatrix matrix)
        {
            SetNormalizer(matrix);

            Single value = 0;
            for (int day = 0; day < matrix.Days; day++)
                for (int slot = 0; slot < matrix.Slots; slot++)
                {
                    var all = matrix[day, slot];
                    if ((!all.Forced) && (all.ChosenItem.HasValue))
                        value += (slotValues != null ? slotValues[slot] : 1);
                }

            var result = value / lastNormalizer;

            Debug.Assert(result >= 0 && result <= 1);

            return result;
        }

        private void SetNormalizer(ShiftMatrix matrix)
        {
            //set the normalizer the first time
            if ((lastMatrix == null) || !object.ReferenceEquals(matrix, lastMatrix))
            {
                lastMatrix = matrix;
                lastNormalizer = 0;
                for (int day = 0; day < matrix.Days; day++)
                    for (int slot = 0; slot < matrix.Slots; slot++)
                    {
                        var all = matrix[day, slot];
                        if (!all.Forced)
                            lastNormalizer += (slotValues != null ? slotValues[slot] : 1);
                    }
            }
        }
    }
}
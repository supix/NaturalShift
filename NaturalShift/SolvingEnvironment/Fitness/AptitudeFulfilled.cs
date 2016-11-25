using NaturalShift.SolvingEnvironment.Matrix;
using System;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class AptitudeFulfilled : IFitnessDimension
    {
        private ShiftMatrix lastMatrix = null;
        private Single lastNormalizer;

        public float Evaluate(ShiftMatrix matrix)
        {
            SetNormalizer(matrix);

            Single value = 0;
            for (int day = 0; day < matrix.Days; day++)
                for (int slot = 0; slot < matrix.Slots; slot++)
                {
                    var all = matrix[day, slot];
                    if ((!all.Forced) && (all.ChosenItem.HasValue))
                        value += all.CurrentAptitudes[all.ChosenItem.Value];
                }
            return value / lastNormalizer;
        }

        private void SetNormalizer(ShiftMatrix matrix)
        {
            if ((lastMatrix == null) || !object.ReferenceEquals(matrix, lastMatrix))
            {
                lastNormalizer = 0;
                for (int day = 0; day < matrix.Days; day++)
                    for (int slot = 0; slot < matrix.Slots; slot++)
                    {
                        if (!matrix[day, slot].Forced)
                            lastNormalizer += matrix[day, slot].InitialAptitudes.Average();
                    }
            }
        }
    }
}
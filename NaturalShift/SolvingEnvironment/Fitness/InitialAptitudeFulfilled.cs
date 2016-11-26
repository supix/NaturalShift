using NaturalShift.SolvingEnvironment.Matrix;
using NaturalShift.SolvingEnvironment.Utils;
using System;
using System.Diagnostics;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.Fitness
{
    internal class InitialAptitudeFulfilled : IFitnessDimension
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
                        value += all.InitialAptitudes[all.ChosenItem.Value];
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
                        if (!matrix[day, slot].Forced)
                            lastNormalizer += matrix[day, slot].InitialAptitudes.Max();
                    }
            }
        }
    }
}
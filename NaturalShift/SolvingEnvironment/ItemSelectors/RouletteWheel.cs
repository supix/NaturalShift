using System;
using System.Linq;

namespace NaturalShift.SolvingEnvironment.ItemSelectors
{
    internal class RouletteWheel : IItemSelector
    {
        /// <summary>
        /// Select an item according to roulette wheel method
        /// </summary>
        /// <param name="candidates">The roulette sector widths</param>
        /// <param name="rouletteValue">The roll value (from 0 to 1)</param>
        /// <returns>The selected item</returns>
        public int? SelectItem(Single[] candidates, Single rouletteValue)
        {
            Single sum = candidates.Sum();

            if (sum == 0)
                return null;

            Single r = (Single)rouletteValue * sum;

            int i = 0;
            while (true)
            {
                r -= candidates[i];
                if ((r <= 0) && (candidates[i] > 0))
                    return i;
                i++;

                //handle cumulative rounding errors which might let i grow beyond vector length
                if (i >= candidates.Length)
                    return i - 1;
            }
        }
    }
}
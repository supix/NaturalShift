// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
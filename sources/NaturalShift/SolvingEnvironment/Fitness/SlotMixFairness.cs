// NaturalShift is an AI based engine to compute workshifts.
// Copyright (C) 2016 - Marcello Esposito (esposito.marce@gmail.com)
//
// This file is part of NaturalShift.
// NaturalShift is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// NaturalShift is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using NaturalShift.SolvingEnvironment.Matrix;
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